using InvestmentPerformanceWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentPerformanceWebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users/{userId}/[controller]")]
    [ApiVersion("1.0")]
    public class InvestmentController : ControllerBase
    {
        private readonly InvestmentDbContext _context;
        private readonly ILogger<InvestmentController> _logger;

        public InvestmentController(InvestmentDbContext context, ILogger<InvestmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/v1/users/{userId}/investments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetInvestmentsForUser(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching investments for user {UserId}", userId);

                var user = await _context.Users
                    .Include(u => u.Investments)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found", userId);
                    return NotFound($"User {userId} not found");
                }

                var investments = user.Investments.Select(i => new
                {
                    i.InvestmentId,
                    i.Name
                });

                return Ok(investments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred when fetching investments for user {userId}", userId);
                return StatusCode(500, "An unepected error occurred, please try again later.");
            }

        }

        // GET: api/v1/users/{userId}/investments/{investmentId}
        [HttpGet("{investmentId}")]
        public async Task<ActionResult<object>> GetInvestmentDetails(int userId, int investmentId)
        {
            try
            {
                _logger.LogInformation("Fetching details for investment {investmentId} for user {userId}", investmentId, userId);

                var investment = await _context.Investments
                    .Where(i => i.UserId == userId && i.InvestmentId == investmentId)
                    .FirstOrDefaultAsync();

                if (investment == null)
                {
                    _logger.LogWarning("Investment {investmentId} not found for user {userId}", investmentId, userId);
                    return NotFound($"Investment {investmentId} not found for user {userId}");
                }

                var result = new
                {
                    investment.InvestmentId,
                    investment.Name,
                    investment.Ticker,
                    investment.NumberOfShares,
                    investment.CostBasisPerShare,
                    investment.CurrentPrice,
                    investment.CurrentValue,
                    investment.Term,
                    investment.TotalGainLoss
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred when fetching investment {investmentId} details for user {userId}", investmentId, userId);
                return StatusCode(500, "An unepected error occurred, please try again later.");
            }
            
        }
    }
}