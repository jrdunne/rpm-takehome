using Microsoft.EntityFrameworkCore;

namespace WeeklyFuelPricesLoader.Database
{
    public interface IFuelPricesContextFactory
    {
        FuelPricesContext Context { get; }
    }

    public class FuelPricesContextFactory : IFuelPricesContextFactory
    {
        public FuelPricesContext Context => new FuelPricesContext();
    }
}
