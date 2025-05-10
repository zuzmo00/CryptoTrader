namespace Crypro.DTO
{
    public class CryptoTradeDtoToFunc
    {
        public string UserId { get; set; }
        public string CryptoId { get; set; }
        public double Amount { get; set; }
    }
    public class CryptoTradeDto
    {
        public string CryptoId { get; set; }
        public double Amount { get; set; }
    }
}