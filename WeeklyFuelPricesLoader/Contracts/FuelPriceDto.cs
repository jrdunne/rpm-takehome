using System.Text.Json.Serialization;

namespace WeeklyFuelPricesLoader.Contracts
{
    public class FuelPriceDto
    {
        [JsonPropertyName("period")]
        public string? Period { get; set; }

        [JsonPropertyName("series")]
        public string? Series { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }

    }
}
