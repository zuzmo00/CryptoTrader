using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{


    public interface IWalletService
    {
        // Define methods for wallet management
        Task<WalletGetDto> GetWallet(string id);
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

        public async Task<WalletGetDto> GetWallet(string id)
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
