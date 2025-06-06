using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface IConvertService
    {
        public Task<ConvertDto> ConvertAsync(ConvertDto convertDto);
    }

    public class ConvertService : IConvertService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        public ConvertService(IMapper mapper, AppDbContext dbContext,IServiceProvider service)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _serviceProvider = service;
        }
        public async Task<ConvertDto> ConvertAsync(ConvertDto convertDto)
        {
            var fromCrypto = await _dbContext.Cryptos
                .FirstOrDefaultAsync(x => x.Id.ToString() == convertDto.FromCurrency)
                ?? throw new Exception($"Crypto not found with id: {convertDto.FromCurrency}");
            var toCrypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id.ToString() == convertDto.ToCurrency)
                ?? throw new Exception($"Crypto not found with id: {convertDto.ToCurrency}");
            var wallet = await _dbContext.Wallets
                .Where(x => x.UserId.ToString() == convertDto.UserId && x.CryptoPockets.Any(x => x.CryptoId.ToString() == convertDto.FromCurrency))
                .Include(x => x.CryptoPockets)
                .ThenInclude(x => x.Crypto)
                .SingleOrDefaultAsync();
            if (wallet== null)
            {
                throw new Exception($"Crypto not found with id: {convertDto.UserId}");
            }


            var fromCryptoPocket = wallet.CryptoPockets
                .FirstOrDefault(cp => cp.CryptoId.ToString().ToUpper() == convertDto.FromCurrency);
                

            var toCryptoPocket = wallet.CryptoPockets
                .FirstOrDefault(cp => cp.CryptoId.ToString().ToUpper() == convertDto.ToCurrency);

            if (fromCryptoPocket == null)
            {
                throw new Exception($"No crypto pocket found for currency: {convertDto.FromCurrency}");
            }

            double fromCryptoAmount = fromCryptoPocket.Amount;

            double fromPrice = fromCrypto.value * fromCryptoAmount;
            double toPrice = toCrypto.value * convertDto.Amount;
            if (fromPrice < toPrice)
            {
                throw new Exception($"Not enough crypto in wallet");
            }

            fromCryptoPocket.Amount -= convertDto.Amount;
            _dbContext.CryptoPockets.Update(fromCryptoPocket);
            if(toCryptoPocket == null)
            {
                var cryptoPocket = new CryptoPocket
                {
                    CryptoId = toCrypto.Id,
                    Amount = convertDto.Amount * fromCrypto.value / toCrypto.value,
                    WalletId = wallet.Id,
                    Value = toCrypto.value
                };
                await _dbContext.CryptoPockets.AddAsync(cryptoPocket);
            }
            else
            {
                toCryptoPocket.Amount += convertDto.Amount*fromCrypto.value/toCrypto.value;
                _dbContext.CryptoPockets.Update(toCryptoPocket);
                var tranLog = new TradeLog
                {
                    UserId = Guid.Parse(convertDto.UserId),
                    CryptoId = toCrypto.Id,
                    Amount = convertDto.Amount,
                    Value = toCrypto.value,
                    IsBuy = Enums.TradeType.Convert,
                    Date = DateTime.UtcNow,

                };
                await _dbContext.TradeLogs.AddAsync(tranLog);
            }
            await _dbContext.SaveChangesAsync();
            return convertDto;
        }
    }
}
