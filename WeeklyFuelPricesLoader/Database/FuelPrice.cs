namespace WeeklyFuelPricesLoader.Database
{
    public class FuelPrice
    {
        public int Id { get; set; }

        // This appears to be a date field, but it was not specified what type to use, so I am defaulting to the source type of a json string field
        public string Period { get; set; } = "";

        public double Value { get; set; }
    }
}
