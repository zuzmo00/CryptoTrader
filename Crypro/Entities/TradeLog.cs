using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
   
    public class TradeLog
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid CryptoId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public Enums.TradeType IsBuy { get; set; } 
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
