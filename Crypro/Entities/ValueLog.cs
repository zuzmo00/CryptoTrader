using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crypro.Entities
{
    public class ValueLog
    {
        [Required,Key]
        public Guid Id { get; set; } = new Guid();
        [ForeignKey("Crypto")]
        public Guid CryptoId { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
