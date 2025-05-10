using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface ICryptoTradeService
    {
        Task<string> BuyCrypto(CryptoTradeDto cryptoTradeDto);
        Task<string> SellCrypto(CryptoTradeDto cryptoTradeDto);
    }
    public class CryptoTradeService : ICryptoTradeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public CryptoTradeService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<string> BuyCrypto(CryptoTradeDto cryptoTradeDto)
        {
            var user = _dbContext.Users.Include(x => x.Wallet).FirstOrDefault(x => x.Id == cryptoTradeDto.UserId) ?? throw new Exception($"User not found with id: {cryptoTradeDto.UserId}");
            var crypto = _dbContext.Cryptos.FirstOrDefault(x => x.Id == cryptoTradeDto.CryproId) ?? throw new Exception($"Crypto not found with id: {cryptoTradeDto.CryproId}");
            double totalCost = cryptoTradeDto.Amount * crypto.value;
            if (user != null && crypto != null)
            {
                var cryptoPocket = _dbContext.CryptoPockets.FirstOrDefaultAsync(x => x.WalletId == user.Wallet.Id && x.CryptoId == crypto.Id).Result;
                if (cryptoPocket == null)
                {
                    if (user.Wallet.Balance < totalCost)
                    {
                        throw new Exception($"Not enough balance in wallet");
                    }
                    else
                    {
                        var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == user.Id) ?? throw new Exception($"Wallet not found with id: {user.Id}");

                        var newCryptoPocket = new CryptoPocket
                        {
                            Amount = cryptoTradeDto.Amount,
                            CryptoId = crypto.Id,
                            WalletId = user.Wallet.Id,
                            Value = crypto.value,

                        };
                        var tranLog= new TradeLog
                        {
                            CryptoId = crypto.Id,
                            Amount = cryptoTradeDto.Amount,
                            UserId = user.Id,
                            Date = DateTime.UtcNow,
                            Value = crypto.value,
                            IsBuy = true,
                        };
                        wallet.Balance -= totalCost;
                        _dbContext.Wallets.Update(wallet);
                        await _dbContext.CryptoPockets.AddAsync(newCryptoPocket);
                        await _dbContext.TradeLogs.AddAsync(tranLog);
                        await _dbContext.SaveChangesAsync();
                        return $"Crypto bought successfully, remaining balance: {user.Wallet.Balance}";
                    }
                }
                else
                {
                    if (user.Wallet.Balance < totalCost)
                    {
                        throw new Exception($"Not enough balance in wallet");
                    }
                    else
                    {
                        var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == user.Id) ?? throw new Exception($"Wallet not found with id: {user.Id}");
                        cryptoPocket.Amount += cryptoTradeDto.Amount;
                        var tranLog = new TradeLog
                        {
                            CryptoId = crypto.Id,
                            Amount = cryptoTradeDto.Amount,
                            UserId = user.Id,
                            Date = DateTime.UtcNow,
                            Value = crypto.value,
                            IsBuy = true,
                        };
                        wallet.Balance -= totalCost;
                        _dbContext.Wallets.Update(wallet);
                        _dbContext.CryptoPockets.Update(cryptoPocket);
                        await _dbContext.TradeLogs.AddAsync(tranLog);
                        await _dbContext.SaveChangesAsync();
                        return $"Crypto bought successfully, remaining balance: {user.Wallet.Balance}";
                    }
                }
            }
            else
            {
                throw new Exception($"User or crypto not found");
            }
        }

        public Task<string> SellCrypto(CryptoTradeDto cryptoTradeDto )
        {
            throw new NotImplementedException();
        }
    }
}
