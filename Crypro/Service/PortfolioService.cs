using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface IPortfolioService
    {
        Task<List<PortfolioDto>> GetUserPortfolio(string userId);
    }
    public class PortfolioService:IPortfolioService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public PortfolioService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<PortfolioDto>> GetUserPortfolio(string userId)
        {
            var wallet=await _dbContext.Wallets
                .Include(x => x.CryptoPockets)
                .ThenInclude(x => x.Crypto)
                .FirstOrDefaultAsync(x => x.UserId.ToString() == userId) ?? throw new Exception($"Wallet not found with id: {userId}");
            var response = _mapper.Map<List<PortfolioDto>>(wallet.CryptoPockets);
            return response;
        }
    }
}
