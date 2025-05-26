namespace Crypro.DTO
{
    public class AddFeeDto
    {
        public double Amount { get; set; }
    }
    public class ChangeFeeDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
    }
}
