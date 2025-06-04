using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; } 
        [Required]
        public string Password { get; set; } 
        [Required]
        public UserRole Role { get; set; } = UserRole.User;
        public Wallet Wallet { get; set; }=new Wallet();
        public List<FeeLog> list { get; set; }
        public bool HasWallet { get; set; } = false;

    }
}
