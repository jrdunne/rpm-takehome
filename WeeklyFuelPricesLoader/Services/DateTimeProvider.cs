namespace WeeklyFuelPricesLoader.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcDateTimeNow { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcDateTimeNow => DateTime.UtcNow;
    }
}
