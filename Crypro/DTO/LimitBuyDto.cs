namespace Crypro.DTO
{
    public class LimitBuyDto
    {
        public Guid UserId { get; set; }
        public Guid CryptoId { get; set; }
        public double Amount { get; set; }
        public double Limit { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
    public class LimitBuyDtoToController
    {
        public Guid CryptoId { get; set; }
        public double Amount { get; set; }
        public double Limit { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
    public class LimitSellDto
    {
        public Guid UserId { get; set; }
        public Guid CryptoId { get; set; }
        public double Amount { get; set; }
        public double Limit { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
    public class LimitSellDtoToController
    {
        public Guid CryptoId { get; set; }
        public double Amount { get; set; }
        public double Limit { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
