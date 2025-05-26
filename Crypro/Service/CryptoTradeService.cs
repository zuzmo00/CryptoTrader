using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Crypro.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Crypro.Service
{
    public interface ICryptoTradeService
    {
        Task<string> BuyCrypto(CryptoTradeDtoToFunc cryptoTradeDto);
        Task<string> SellCrypto(CryptoTradeDtoToFunc cryptoTradeDto);
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
        public async Task<string> BuyCrypto(CryptoTradeDtoToFunc cryptoTradeDto)
        {
            var fee = _dbContext.TransactionFees.First();
            var user = _dbContext.Users.Include(x => x.Wallet).FirstOrDefault(x => x.Id.ToString() == cryptoTradeDto.UserId) ?? throw new Exception($"User not found with id: {cryptoTradeDto.UserId}");
            var crypto = _dbContext.Cryptos.FirstOrDefault(x => x.Id.ToString() == cryptoTradeDto.CryptoId) ?? throw new Exception($"Crypto not found with id: {cryptoTradeDto.CryptoId}");
            double totalCost = (cryptoTradeDto.Amount * crypto.value)*fee.Amount;
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
                        var tranLog = new TradeLog
                        {
                            CryptoId = crypto.Id,
                            Amount = cryptoTradeDto.Amount,
                            UserId = user.Id,
                            Date = DateTime.UtcNow,
                            Value = crypto.value,
                            IsBuy = true,
                        };
                        var feeLog = new FeeLog
                        {
                            UserId = user.Id,
                            TransactionId = tranLog.Id,
                            CryptoId = crypto.Id,
                            Amount = cryptoTradeDto.Amount,
                            FeePercentage = fee.Amount,
                            Timestamp = DateTime.UtcNow,
                            TotalAmount = totalCost
                        };
                        wallet.Balance -= totalCost;
                        _dbContext.Wallets.Update(wallet);
                        await _dbContext.CryptoPockets.AddAsync(newCryptoPocket);
                        await _dbContext.TradeLogs.AddAsync(tranLog);
                        await _dbContext.FeeLogs.AddAsync(feeLog);
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
                        var feeLog = new FeeLog
                        {
                            UserId = user.Id,
                            TransactionId = tranLog.Id,
                            CryptoId = crypto.Id,
                            Amount = cryptoTradeDto.Amount,
                            FeePercentage = fee.Amount,
                            Timestamp = DateTime.UtcNow,
                            TotalAmount = totalCost
                        };
                        wallet.Balance -= totalCost;
                        _dbContext.Wallets.Update(wallet);
                        _dbContext.CryptoPockets.Update(cryptoPocket);
                        await _dbContext.TradeLogs.AddAsync(tranLog);
                        await _dbContext.FeeLogs.AddAsync(feeLog);
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

        public async Task<string> SellCrypto(CryptoTradeDtoToFunc cryptoTradeDto)
        {
            var fee = _dbContext.TransactionFees.First();
            var user = await _dbContext.Users.Include(x => x.Wallet).FirstOrDefaultAsync(x => x.Id.ToString() == cryptoTradeDto.UserId) ?? throw new Exception($"User not found with id: {cryptoTradeDto.UserId}");
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id.ToString() == cryptoTradeDto.CryptoId) ?? throw new Exception($"Crypto not found with id: {cryptoTradeDto.CryptoId}");
            if (user != null && crypto != null)
            {
                var cryptoPocket = await _dbContext.CryptoPockets.FirstOrDefaultAsync(x => x.WalletId == user.Wallet.Id && x.CryptoId == crypto.Id) ?? throw new Exception($"Crypto pocket not found with id: {cryptoTradeDto.CryptoId}");
                if (cryptoPocket.Amount < cryptoTradeDto.Amount)
                {
                    throw new Exception($"Not enough crypto in wallet");
                }
                else
                {
                    var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == user.Id) ?? throw new Exception($"Wallet not found with id: {user.Id}");
                    double totalCost = (cryptoTradeDto.Amount * crypto.value)- (cryptoTradeDto.Amount * crypto.value)*1-fee.Amount;
                    cryptoPocket.Amount -= cryptoTradeDto.Amount;
                    var tranLog = new TradeLog
                    {
                        CryptoId = crypto.Id,
                        Amount = cryptoTradeDto.Amount,
                        UserId = user.Id,
                        Date = DateTime.UtcNow,
                        Value = crypto.value,
                        IsBuy = false,
                    };
                    var feeLog = new FeeLog
                    {
                        UserId = user.Id,
                        TransactionId = tranLog.Id,
                        CryptoId = crypto.Id,
                        Amount = cryptoTradeDto.Amount,
                        FeePercentage = fee.Amount,
                        Timestamp = DateTime.UtcNow,
                        TotalAmount = totalCost
                    };
                    wallet.Balance += totalCost;
                    _dbContext.Wallets.Update(wallet);
                    _dbContext.CryptoPockets.Update(cryptoPocket);
                    await _dbContext.TradeLogs.AddAsync(tranLog);
                    await _dbContext.FeeLogs.AddAsync(feeLog);
                    await _dbContext.SaveChangesAsync();
                    return $"Crypto sold successfully, remaining balance: {user.Wallet.Balance}";
                }
            }
            else
            {
                throw new Exception($"User or crypto not found");
            }
        }
    }
}
