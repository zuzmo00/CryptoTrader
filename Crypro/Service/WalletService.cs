using AutoMapper;
using Crypro.Context;

namespace Crypro.Service
{


    public interface IWalletService
    {
        // Define methods for wallet management
        Task<string> GetWallet(string id);
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
        public Task<string> GetWallet(string id)
        {
            var wallet= _dbContext.Wallets.FirstOrDefault(x => x.UserId.ToString() == id);
        }
    }
}
