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
        [Required,Key]
        public Guid Id { get; set; }= new Guid();
        [Required]
        public string Name { get; set; }= string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; } = UserRole.User;
        public Wallet Wallet { get; set; }=new Wallet();
        public bool HasWallet { get; set; } = false;

    }
}
