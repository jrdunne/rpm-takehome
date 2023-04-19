using Moq;
using WeeklyFuelPricesLoader.Contracts;
using WeeklyFuelPricesLoader.Database;
using WeeklyFuelPricesLoader.Services;

namespace WeeklyFuelPricesLoader.Tests
{
    public static class TestHelpers
    {
        public static IDateTimeProvider GetMockDateTimeProvider(DateTime value)
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.UtcDateTimeNow).Returns(value);

            return dateTimeProviderMock.Object;
        }

        public static IFuelPricesApi GetMockFuelPricesApi(FuelPriceResponseDto value)
        {
            var fuelPricesApiMock = new Mock<IFuelPricesApi>();
            fuelPricesApiMock.Setup(x => x.GetFuelPricesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(value));

            return fuelPricesApiMock.Object;
        }

        public static IFuelPricesContextFactory GetInMemoryDbContextFactory()
        {
            var contextFactory = new Mock<IFuelPricesContextFactory>();
            contextFactory.Setup(x => x.Context).Returns(new TestFuelPricesContext());

            return contextFactory.Object;
        }
        
    }
}
