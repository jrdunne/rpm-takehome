using System.Text.Json.Serialization;

namespace WeeklyFuelPricesLoader.Contracts
{
    internal class EiaApiResponseDto
    {
        [JsonPropertyName("response")]
        public FuelPriceResponseDto Response { get; set; }
    }
}
