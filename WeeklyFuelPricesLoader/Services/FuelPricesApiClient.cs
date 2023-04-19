using System.Text.Json;
using WeeklyFuelPricesLoader.Contracts;

namespace WeeklyFuelPricesLoader.Services
{
    public interface IFuelPricesApi
    {
        Task<FuelPriceResponseDto> GetFuelPricesAsync(CancellationToken cancellationToken);
    }

    public class FuelPricesApiClient : IFuelPricesApi
    {
        // One instance shared for the lifetime of the app
        private static HttpClient _httpClient = new HttpClient();

        // This url and it's parameters probably should be config driven or a parameter to the GetFuelPrices method
        private const string RequestUrl = "https://api.eia.gov/v2/petroleum/pri/gnd/data/?frequency=weekly&data[0]=value&facets[series][]=EMD_EPD2D_PTE_NUS_DPG&sort[0][column]=period&sort[0][direction]=desc&offset=0&length=5000&api_key=EthXWE6eUTrBEJ1uTpNCqbL4NjghRxaC2R5tw1b2";

        /// <summary>
        /// Calls api.eia.gov for fuel prices using specified url. Returns deserialized response
        /// </summary>
        public async Task<FuelPriceResponseDto> GetFuelPricesAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(RequestUrl, cancellationToken);

            if (!response.IsSuccessStatusCode) throw new Exception("There was no error handling in the spec, so we shall bubble up exceptions and crash the app");

            using var responseContentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (responseContentStream == null) throw new Exception("There was no error handling in the spec, so we shall bubble up exceptions and crash the app");

            var fuelResponse = await JsonSerializer.DeserializeAsync<EiaApiResponseDto>(responseContentStream);

            if (fuelResponse == null) throw new Exception("There was no error handling in the spec, so we shall bubble up exceptions and crash the app");

            return fuelResponse.Response;
        }
    }
}
