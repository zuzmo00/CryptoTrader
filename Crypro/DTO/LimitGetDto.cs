using Crypro.Entities;

namespace Crypro.DTO
{
    public class LimitGetDto
    {
        public double Limit { get; set; }
        public double Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public TradeType Type { get; set; }
    }
}
