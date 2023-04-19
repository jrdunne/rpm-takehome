using Microsoft.EntityFrameworkCore;
using WeeklyFuelPricesLoader.Configuration;
using WeeklyFuelPricesLoader.Contracts;
using WeeklyFuelPricesLoader.Database;

namespace WeeklyFuelPricesLoader.Services
{
    public interface IFuelPricesLoader
    {
        Task LoadFuelPricesAsync(CancellationToken cancellationToken = default);
    }

    public class FuelPricesLoader : IFuelPricesLoader
    {
        // Used to bypass hangfire activator problems since i am avoiding DI
        public static IFuelPriceLoaderConfiguration StaticConfig = null;

        private const string DateFormat = "yyyy-MM-dd";
        private const string ExpectedDateFormatResponse = "YYYY-MM-DD";

        private readonly IFuelPricesApi _fuelPricesApi;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IFuelPriceLoaderConfiguration _config;
        private readonly IFuelPricesContextFactory _contextFactory;

        public FuelPricesLoader() : this(StaticConfig, new FuelPricesApiClient(), new DateTimeProvider(), new FuelPricesContextFactory())
        {
        }

        public FuelPricesLoader(IFuelPriceLoaderConfiguration config) : this(config, new FuelPricesApiClient(), new DateTimeProvider(), new FuelPricesContextFactory())
        {
        }

        public FuelPricesLoader(IFuelPriceLoaderConfiguration config, IFuelPricesApi fuelPricesApi, IDateTimeProvider dateTimeProvider, IFuelPricesContextFactory contextFactory)
        {
            _fuelPricesApi = fuelPricesApi;
            _dateTimeProvider = dateTimeProvider;
            _config = config;
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Calls the fuel prices api client, converts to db model, checks for duplicates, then inserts results
        /// </summary>
        public async Task LoadFuelPricesAsync(CancellationToken cancellationToken = default)
        {
            // get prices
            var fuelPriceResponse = await _fuelPricesApi.GetFuelPricesAsync(cancellationToken);

            // filter to only prices more recent or equal to N days ago
            var filteredPrices = FilterFuelPriceDtos(fuelPriceResponse);

            // convert prices
            var convertedPrices = ConvertFuelPrices(filteredPrices);

            // Save fuel prices
            await SaveFuelPricesAsync(convertedPrices, cancellationToken);
        }

        /// <summary>
        /// Filters fuel prices based on look back period
        /// </summary>
        public IList<FuelPriceDto> FilterFuelPriceDtos(FuelPriceResponseDto response)
        {
            // check to make sure date format is correct
            if (response.DateFormat != ExpectedDateFormatResponse) throw new Exception("Unexpected date format");
            
            var oldestDate = _dateTimeProvider.UtcDateTimeNow.AddDays(-1 * _config.MaxLookbackDays);

            var validPrices = response.Data.Where(price =>
            {
                if (!DateTime.TryParseExact(price.Period, DateFormat, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out var pricePeriodDate))
                {
                    throw new Exception("This might not be an error, but it is not specified");
                }

                // valid periods are always >= current date - lookback days
                return pricePeriodDate.Date >= oldestDate.Date;
            }).ToList();

            return validPrices;
        }

        /// <summary>
        /// Converts fuel prices to db model
        /// </summary>
        public IList<FuelPrice> ConvertFuelPrices(IList<FuelPriceDto> prices)
        {
            var dbPrices = prices.Select(price => new FuelPrice
            {
                Period = price.Period,
                Value = price.Value
            }).ToList();

            return dbPrices;
        }

        /// <summary>
        /// Checks for duplicates and then saves new records to database
        /// </summary>
        public async Task SaveFuelPricesAsync(IList<FuelPrice> fuelPrices, CancellationToken cancellationToken)
        {
            using (var context = _contextFactory.Context)
            {
                // Get existing periods. If they are in the db they are distinct since there is a unqiue index
                var existingPeriods = await context.Set<FuelPrice>().Select(x => x.Period).AsNoTracking().ToListAsync();

                var existingPeriodsSet = existingPeriods.ToHashSet();

                var newFuelPrices = fuelPrices.Where(x => !existingPeriods.Contains(x.Period));

                // Save new fuel prices
                await context.AddRangeAsync(newFuelPrices, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
