using InvestmentPerformanceWebAPI.Controllers;
using InvestmentPerformanceWebAPI.Data;
using InvestmentPerformanceWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InvestmentPerformanceWebAPI.Tests.Controllers
{
    public class InvestmentControllerTests
    {
        private InvestmentDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<InvestmentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new InvestmentDbContext(options);

            var user = new User
            {
                UserId = 1,
                Name = "Alice",
                Investments = new List<Investment>
                {
                    new Investment
                    {
                        InvestmentId = 1,
                        UserId = 1,
                        Name = "Apple Inc.",
                        Ticker = "AAPL",
                        NumberOfShares = 10,
                        CostBasisPerShare = 120,
                        CurrentPrice = 175,
                        TimeOfPurchase = new DateTime(2023, 6, 10)
                    }
                }
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetInvestmentsForUser_ReturnsInvestments_WhenUserExists()
        {
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<InvestmentController>>();
            var controller = new InvestmentController(context, logger);

            var result = await controller.GetInvestmentsForUser(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var investments = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Single(investments);
        }

        [Fact]
        public async Task GetInvestmentDetails_ReturnsCorrectDetails_WhenInvestmentExists()
        {
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<InvestmentController>>();
            var controller = new InvestmentController(context, logger);

            var result = await controller.GetInvestmentDetails(1, 1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var investment = okResult.Value;
            Assert.NotNull(investment);

            var name = investment.GetType().GetProperty("Name")?.GetValue(investment);
            Assert.Equal("Apple Inc.", name);
        }

        [Fact]
        public async Task GetInvestmentsForUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<InvestmentController>>();
            var controller = new InvestmentController(context, logger);

            var result = await controller.GetInvestmentsForUser(13);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("User 13 not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetInvestmentDetails_ReturnsNotFound_WhenInvestmentDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var logger = Mock.Of<ILogger<InvestmentController>>();
            var controller = new InvestmentController(context, logger);

            var result = await controller.GetInvestmentDetails(1, 50);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Investment 50 not found for user 1", notFoundResult.Value);
        }

    }
}
