using WeeklyFuelPricesLoader.Configuration;
using WeeklyFuelPricesLoader.Contracts;
using WeeklyFuelPricesLoader.Services;

namespace WeeklyFuelPricesLoader.Tests
{
    [TestClass]
    public class FuelPricesLoaderTests
    {
        // Example tests. More should be added for the error cases once they are defined.


        private readonly static FuelPriceResponseDto ApiResponse = new FuelPriceResponseDto
        {
            DateFormat = "YYYY-MM-DD",
            Data = new List<FuelPriceDto>
                {
                    new FuelPriceDto
                    {
                        Period = "2023-04-20",
                        Value = 1.0
                    },
                    new FuelPriceDto
                    {
                        Period = "2023-04-19",
                        Value = 2.0
                    },
                    new FuelPriceDto
                    {
                        Period = "2023-04-18",
                        Value = 3.0
                    },
                    new FuelPriceDto
                    {
                        Period = "2023-04-17",
                        Value = 4.0
                    }
                }
        };

        private FuelPricesLoader GetFuelPricesLoader()
        {
            var config = new FuelPriceLoaderConfiguration()
            {
                MaxLookbackDays = 2,
                CronSchedule = ""
            };

            var date = DateTime.ParseExact("2023-04-20", "yyyy-MM-dd", null);
            var loader = new FuelPricesLoader(config, TestHelpers.GetMockFuelPricesApi(ApiResponse), TestHelpers.GetMockDateTimeProvider(date), TestHelpers.GetInMemoryDbContextFactory());
            return loader;
        }
        

        [TestMethod]
        public void FilterFuelPriceDtos_Test()
        {
            var loader = GetFuelPricesLoader();

            // should filter all records older than 04-18
            var filtered = loader.FilterFuelPriceDtos(ApiResponse).ToList();

            Assert.AreEqual(3, filtered.Count);
            Assert.AreEqual("2023-04-20", filtered[0].Period);
            Assert.AreEqual("2023-04-19", filtered[1].Period);
            Assert.AreEqual("2023-04-18", filtered[2].Period);
        }

        [TestMethod]
        public void ConvertFuelPrices_Test()
        {
            var loader = GetFuelPricesLoader();

            // should filter all records older than 04-18
            var converted = loader.ConvertFuelPrices(ApiResponse.Data).ToList();

            Assert.AreEqual(4, converted.Count);
            Assert.AreEqual("2023-04-20", converted[0].Period);
            Assert.AreEqual(1.0, converted[0].Value);
            Assert.AreEqual("2023-04-17", converted[3].Period);
            Assert.AreEqual(4.0, converted[3].Value);
        }

        // End to end test showing mocks and in memory db context
        // Should add test for dedup logic too
        [TestMethod]
        public async Task SaveFuelPricesAsync_Test()
        {
            var loader = GetFuelPricesLoader();

            await loader.LoadFuelPricesAsync();
        }
    }
}