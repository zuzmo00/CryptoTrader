namespace Crypro.DTO
{
    public class ConvertDto
    {
        public string UserId { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public double Amount { get; set; }
    }
}
