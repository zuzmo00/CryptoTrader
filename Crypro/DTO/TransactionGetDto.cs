namespace Crypro.DTO
{
    public class TransactionGetAllByUserDto
    {
        public Guid Id { get; set; }
        public string CryptoId { get; set; }
        public string UserId { get; set; }
        public double Value { get; set; }
        public double Amount { get; set; }
        public Enums.TradeType IsBuy { get; set; } 
        public DateTime Date { get; set; }
    }
    public class TransactinGetByTransactionId
    {
        public Guid Id { get; set; }
        public string CryptoId { get; set; }
        public string UserId { get; set; }
        public double Value { get; set; }
        public double Amount { get; set; }
        public Enums.TradeType  IsBuy { get; set; } // true for buy, false for sell
        public DateTime Date { get; set; }
    }
}
