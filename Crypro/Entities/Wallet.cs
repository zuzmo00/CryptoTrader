using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crypro.Entities
{
    public class Wallet
    {
        [Required,Key]
        public Guid Id { get; set; } = new Guid();
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [Required]
        public double Balance { get; set; }=1000.0;
        public List<CryptoPocket> CryptoPockets { get; set; } = new List<CryptoPocket>();
    }
}
