using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface ICryptoManagerService
    {
        public Task<List<CryptoGetDto>> ListCryptos();
        public Task<CryptoGetDto> GetCryptoById(Guid Cryptoid);
        public Task<Guid>CryptoCreate(CryptoCreateDto cryptoCreateDto);
        public Task<string> RemoveCrypto(Guid CryptoId);

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

        public async Task<Guid> CryptoCreate(CryptoCreateDto cryptoCreateDto)
        {
            var crypto = _mapper.Map<Crypto>(cryptoCreateDto);
            await _dbContext.Cryptos.AddAsync(crypto);
            await _dbContext.SaveChangesAsync();
            return crypto.Id;
        }

        public async Task<CryptoGetDto> GetCryptoById(Guid Cryptoid)
        {
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id == Cryptoid)?? throw new Exception($"Crypto not found, id:{Cryptoid}");
            return _mapper.Map<CryptoGetDto>(crypto);
        }

        public async Task<List<CryptoGetDto>> ListCryptos()
        {
            var cryptos = await _dbContext.Cryptos.ToListAsync();
            return _mapper.Map<List<CryptoGetDto>>(cryptos);
        }

        public async Task<string> RemoveCrypto(Guid CryptoId)
        {
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id == CryptoId) ?? throw new Exception($"Crypto not found, id:{CryptoId}");
            _dbContext.Cryptos.Remove(crypto);
            await _dbContext.SaveChangesAsync();
            return $"Crypto with id: {CryptoId} deleted successfully";
        }
    }
}
