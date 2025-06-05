using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{


    public interface IWalletService
    {
        Task<WalletGetDto> GetWalletAsync(string id);
        Task<string> AddToBalanceAsync(Guid id, AddToBalanceDto addToBalanceDto);
        Task<string> DeleteWalletAsync(Guid id);
    }
    public class WalletService : IWalletService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public WalletService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string> AddToBalanceAsync(Guid id, AddToBalanceDto addToBalanceDto)
        {
            var Wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == id) ?? throw new Exception($"Nincs iylen felhasználó");
            Wallet.Balance += addToBalanceDto.Amount;
            _dbContext.Wallets.Update(Wallet);
            await _dbContext.SaveChangesAsync();
            return $"A feltöltés sikeres {Wallet.Balance}";
        }

        public async Task<string> DeleteWalletAsync(Guid id)
        {
            var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == id);
            if (wallet == null)
            {
                throw new Exception($"Wallet not found with id: {id}");
            }
            else
            {
                _dbContext.Wallets.Remove(wallet);
                await _dbContext.SaveChangesAsync();
                return $"Wallet with id: {id} deleted successfully";
            }
        }

        public async Task<WalletGetDto> GetWalletAsync(string id)
        {
            var wallet=await _dbContext.Wallets
                .Include(x=>x.CryptoPockets)
                .ThenInclude(x=>x.Crypto)
                .FirstOrDefaultAsync(x=>x.UserId.ToString() == id)?? throw new Exception($"Wallet not fouind{id}");
            var response=_mapper.Map<WalletGetDto>(wallet);
            response.CryptoPockets=_mapper.Map<List<CryptoPocketDto>>(wallet.CryptoPockets);
            return response;

        }
    }
}
