using Microsoft.EntityFrameworkCore;
using WeeklyFuelPricesLoader.Database;

namespace WeeklyFuelPricesLoader.Tests
{

    internal class TestFuelPricesContext : FuelPricesContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Local sql server express connection string
            optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        }
    }
}
