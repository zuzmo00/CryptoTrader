using Crypro.Context;
using Crypro.DTO;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface IProfitService
    {
        Task<double> GetUserProfit(string userId);
        Task<List<ProfitDto>> GetProfitByCrypto(string userId);
    }
    public class ProfitService : IProfitService
    {
        private readonly AppDbContext _dbContext;
        public ProfitService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProfitDto>> GetProfitByCrypto(string userId)
        {
            List<ProfitDto> profit = new List<ProfitDto>();
            var wallet = await _dbContext.Wallets
                .Include(x => x.CryptoPockets)
                .ThenInclude(cp => cp.Crypto)
                .FirstOrDefaultAsync(x => x.UserId.ToString() == userId) ?? throw new Exception($"Wallet not found with id: {userId}");
            if (wallet == null)
            {
                throw new Exception($"There is no user with id:{userId}");
            }
            foreach(var item in wallet.CryptoPockets)
            {
                profit.Add(new ProfitDto
                {
                    CryptoName = item.Crypto.Name,
                    Amount = (item.Crypto.value - item.Value) * item.Amount,
                });
            }
            return profit;
        }

        public async Task<double> GetUserProfit(string userId)
        {
            var wallet = await _dbContext.Wallets
            .Include(x => x.CryptoPockets)
            .ThenInclude(cp => cp.Crypto)
            .FirstOrDefaultAsync(x => x.UserId.ToString() == userId) ?? throw new Exception($"Wallet not found with id: {userId}");

            if (wallet == null)
            {
                throw new Exception($"There is no user with id:{userId}");
            }
            return  wallet.CryptoPockets.Sum(u => (u.Crypto.value - u.Value) * u.Amount);
        }

    }
}
