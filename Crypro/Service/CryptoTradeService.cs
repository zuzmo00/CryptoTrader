using Crypro.DTO;

namespace Crypro.Service
{
    public interface ICryptoTradeService
    {
        Task<string> BuyCrypto(CryptoTradeDto cryptoTradeDto);
        Task<string> SellCrypto(CryptoTradeDto cryptoTradeDto);
    }
    public class CryptoTradeService : ICryptoTradeService
    {
        public Task<string> BuyCrypto(CryptoTradeDto cryptoTradeDto)
        {
            throw new NotImplementedException();
        }

        public Task<string> SellCrypto(CryptoTradeDto cryptoTradeDto )
        {
            throw new NotImplementedException();
        }
    }
}
