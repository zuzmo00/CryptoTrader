using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
    public class Crypto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } 
        [Required]
        public double value { get; set; }
    }
}
