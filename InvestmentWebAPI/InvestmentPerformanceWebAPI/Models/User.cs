
namespace InvestmentPerformanceWebAPI.Models
{
    public class User 
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Investment> Investments { get; set; } = new List<Investment>(); 
    }
}