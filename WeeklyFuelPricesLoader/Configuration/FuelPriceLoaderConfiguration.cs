namespace WeeklyFuelPricesLoader.Configuration
{
    public interface IFuelPriceLoaderConfiguration
    {
        int MaxLookbackDays { get; }
        string CronSchedule { get; }
    }

    public class FuelPriceLoaderConfiguration : IFuelPriceLoaderConfiguration
    {
        public int MaxLookbackDays { get; set; }
        public string CronSchedule { get; set; } = "";
    }
}
