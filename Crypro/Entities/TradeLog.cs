using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
    public class TradeLog
    {
        [Required, Key]
        public Guid Id { get; set; } = new Guid();
        [Required]
        public Guid CryptoId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public bool IsBuy { get; set; } // true for buy, false for sell
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
