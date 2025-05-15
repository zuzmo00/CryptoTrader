using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
    public class LimitLog
    {
        [Key]
        public Guid Id { get; set; }= new Guid();
        public Guid CryptoId { get; set; }
        public Guid UserId { get; set; }
        public double Limit { get; set; }
        public Double Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public TradeType Type { get; set; } 
    }
}
