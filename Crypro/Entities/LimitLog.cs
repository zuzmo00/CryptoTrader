using System.ComponentModel.DataAnnotations;
using static Crypro.Enums;

namespace Crypro.Entities
{
    public class LimitLog
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CryptoId { get; set; }
        public Guid UserId { get; set; }
        public double Limit { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public TradeType Type { get; set; } 
    }
}
