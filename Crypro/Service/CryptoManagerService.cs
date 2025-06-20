﻿using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface ICryptoManagerService
    {
        public Task<List<CryptoGetDto>> ListCryptosAsync();
        public Task<CryptoGetDto> GetCryptoByIdAsync(Guid Cryptoid);
        public Task<Guid>CryptoCreateAsync(CryptoCreateDto cryptoCreateDto);
        public Task<string> RemoveCryptoAsync(Guid CryptoId);
        public Task<string> UpdateCryptoAsync(CryptoUpdateDto cryptoUpdateDto);

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

        public async Task<Guid> CryptoCreateAsync(CryptoCreateDto cryptoCreateDto)
        {
            var crypto = _mapper.Map<Crypto>(cryptoCreateDto);
            await _dbContext.Cryptos.AddAsync(crypto);
            await _dbContext.SaveChangesAsync();
            return crypto.Id;
        }

        public async Task<CryptoGetDto> GetCryptoByIdAsync(Guid Cryptoid)
        {
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id == Cryptoid)?? throw new Exception($"Crypto not found, id:{Cryptoid}");
            return _mapper.Map<CryptoGetDto>(crypto);
        }

        public async Task<List<CryptoGetDto>> ListCryptosAsync()
        {
            var cryptos = await _dbContext.Cryptos.ToListAsync();
            return _mapper.Map<List<CryptoGetDto>>(cryptos);
        }

        public async Task<string> RemoveCryptoAsync(Guid CryptoId)
        {
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id == CryptoId) ?? throw new Exception($"Crypto not found, id:{CryptoId}");
            _dbContext.Cryptos.Remove(crypto);
            await _dbContext.SaveChangesAsync();
            return $"Crypto with id: {CryptoId} deleted successfully";
        }

        public async Task<string> UpdateCryptoAsync(CryptoUpdateDto cryptoUpdateDto)
        {
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id.ToString() == cryptoUpdateDto.CryptoId) ?? throw new Exception($"Crypto not found, id:{cryptoUpdateDto.CryptoId}");
            crypto.value = cryptoUpdateDto.Value;
            _dbContext.Cryptos.Update(crypto);
            await _dbContext.SaveChangesAsync();
            return $"Crypto with id: {cryptoUpdateDto.CryptoId} updated successfully";
        }
    }
}
