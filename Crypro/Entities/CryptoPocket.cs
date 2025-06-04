using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace Crypro.Entities
{
    public class CryptoPocket
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Wallet")]
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }

        [ForeignKey("Crypto")]
        public Guid CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public double Value { get; set; }
    }
}
