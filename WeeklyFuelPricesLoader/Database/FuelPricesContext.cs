using Microsoft.EntityFrameworkCore;

namespace WeeklyFuelPricesLoader.Database
{
    public class FuelPricesContext : DbContext
    {
        public DbSet<FuelPrice> FuelPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Local sql server express connection string
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // The spec said to not have duplicates on period and an index on the db is the best way to enforce that
            modelBuilder.Entity<FuelPrice>()
                .HasIndex(f => f.Period)
                .IsUnique();
        }
    }
}
