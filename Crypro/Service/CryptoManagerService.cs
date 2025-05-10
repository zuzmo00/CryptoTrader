using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface ICryptoManagerService
    {
       public Task<List<CryptoGetDto>> ListCryptos();
    }
    public class CryptoManagerService:ICryptoManagerService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public CryptoManagerService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CryptoGetDto>> ListCryptos()
        {
            var cryptos = await _dbContext.Cryptos.ToListAsync();
            return _mapper.Map<List<CryptoGetDto>>(cryptos);
        }
    }
}
