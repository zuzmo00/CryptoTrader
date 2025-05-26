using System.ComponentModel.DataAnnotations;

namespace Crypro.Entities
{
    public class TransactionFee
    {
        [Key]
        public Guid Id { get; set; }=new Guid();
        public double Amount { get; set; }= 0.2; // Default fee of 0.2%
    }
}
