using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
    public enum TradeType
    {
        Buy,
        Sell
    }
    public class LimitedTransaction
    {
        [Key]
        public Guid Id { get; set; }= new Guid();
        public Guid CryptoId { get; set; }
        public Guid UserId { get; set; }
        public double Limit { get; set; }
        public Double Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public TradeType Type { get; set; }

    }
}
