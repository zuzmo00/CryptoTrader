namespace Crypro.Service
{


    public interface IWalletService
    {
        // Define methods for wallet management
        Task<Guid> CreateWalletAsync(Guid userId);
        Task<bool> UpdateWalletAsync(Guid walletId, WalletUpdateDto walletUpdateDto);
        Task<bool> DeleteWalletAsync(Guid walletId);
        Task<Wallet> GetWalletByIdAsync(Guid walletId);
        Task<List<Wallet>> GetAllWalletsAsync();
    }
    public class WalletService
    {
    }
}
