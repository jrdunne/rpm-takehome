using System.Text.Json.Serialization;

namespace WeeklyFuelPricesLoader.Contracts
{
    public class FuelPriceResponseDto
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("dateFormat")]
        public string? DateFormat { get; set; }

        [JsonPropertyName("frequency")]
        public string? Frequency { get; set; }

        [JsonPropertyName("data")]
        public List<FuelPriceDto> Data { get; set; } = new List<FuelPriceDto>();
    }
}
