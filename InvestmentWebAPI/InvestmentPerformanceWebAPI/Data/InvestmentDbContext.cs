using Microsoft.EntityFrameworkCore;
using InvestmentPerformanceWebAPI.Models;

namespace InvestmentPerformanceWebAPI.Data
{
    public class InvestmentDbContext : DbContext
    {
        public InvestmentDbContext(DbContextOptions<InvestmentDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Investment> Investments => Set<Investment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Investments)
                .WithOne(i => i.User!)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Name = "Alice" },
                new User { UserId = 2, Name = "Bob" }
            );

            modelBuilder.Entity<Investment>().HasData(
                new Investment
                {
                    InvestmentId = 1,
                    UserId = 1,
                    Name = "Apple Inc.",
                    Ticker = "AAPL",
                    NumberOfShares = 10,
                    CostBasisPerShare = 120.00m,
                    CurrentPrice = 175.00m,
                    TimeOfPurchase = new DateTime(2023, 6, 10)
                },
                new Investment
                {
                    InvestmentId = 2,
                    UserId = 1,
                    Name = "Tesla Motors",
                    Ticker = "TSLA",
                    NumberOfShares = 5,
                    CostBasisPerShare = 220.00m,
                    CurrentPrice = 200.00m,
                    TimeOfPurchase = new DateTime(2025, 9, 15)
                },
                new Investment
                {
                    InvestmentId = 3,
                    UserId = 2,
                    Name = "Amazon.com",
                    Ticker = "AMZN",
                    NumberOfShares = 8,
                    CostBasisPerShare = 95.00m,
                    CurrentPrice = 130.00m,
                    TimeOfPurchase = new DateTime(2024, 2, 1)
                }
            );
        }
    }
}