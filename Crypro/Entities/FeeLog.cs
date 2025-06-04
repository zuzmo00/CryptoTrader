using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace Crypro.Entities
{
    public class FeeLog
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid CryptoId { get; set; }
        public double Amount { get; set; }
        public double FeePercentage { get; set; } = 0.2; // Default fee of 0.2%
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double TotalAmount { get; set; }
    }
}
