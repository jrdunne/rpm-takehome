using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Configuration;
using WeeklyFuelPricesLoader.Configuration;
using WeeklyFuelPricesLoader.Services;

GlobalConfiguration.Configuration.UseMemoryStorage().UseColouredConsoleLogProvider();

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("FuelPriceLoaderConfiguration.json", optional: false, reloadOnChange: true);

var config = builder.Build();

// I should add config validation
var fuelPriceLoaderConfig = config.Get<FuelPriceLoaderConfiguration>();
// This is gross, but i was too lazy to add DI, so I need a default ctor for the hangfire expression 
FuelPricesLoader.StaticConfig = fuelPriceLoaderConfig;

using (var server = new BackgroundJobServer())
{
    var fuelPricesLoader = new FuelPricesLoader();
    RecurringJob.AddOrUpdate(() => fuelPricesLoader.LoadFuelPricesAsync(CancellationToken.None), fuelPriceLoaderConfig.CronSchedule);
    await Task.Delay(-1);
}