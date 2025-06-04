using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crypro.Entities
{
    public class ValueLog
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Crypto")]
        public Guid CryptoId { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
