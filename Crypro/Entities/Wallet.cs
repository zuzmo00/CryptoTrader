using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Crypro.Entities
{
    public class Wallet
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public double Balance { get; set; } = 1000.0;
        [JsonIgnore]
        public User User { get; set; }

        public List<CryptoPocket> CryptoPockets { get; set; } = new List<CryptoPocket>();
    }
}
