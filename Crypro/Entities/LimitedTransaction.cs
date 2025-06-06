using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Crypro.Enums;

namespace Crypro.Entities
{
   
    public class LimitedTransaction
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CryptoId { get; set; }
        public Guid UserId { get; set; }
        public double Limit { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public TradeType Type { get; set; }

    }
}
