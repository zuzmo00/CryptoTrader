using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
    public class Crypto
    {
        [Required, Key]
        public Guid Id { get; set; } = new Guid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public double value { get; set; }
    }
}
