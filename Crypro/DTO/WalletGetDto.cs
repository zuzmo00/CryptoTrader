namespace Crypro.DTO
{
    public class WalletGetDto
    {
        public double Balance { get; set; }
        public List<CryptoPocketDto> CryptoPockets { get; set; }
    }
    public class CryptoPocketDto
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double Amount { get; set; }
        
    }
    public class AddToBalanceDto 
    {
        public double Amount { get; set; }
    }
}
