namespace Crypro.DTO
{
    public class WalletGetDto
    {
        public double Value { get; set; }
        public List<CryptoPocketDto> OwnedCryptos { get; set; }
    }
    public class CryptoPocketDto
    {
        public string CryptoName { get; set; }
        public double Value { get; set; }
        public double Amount { get; set; }
        
    }
}
