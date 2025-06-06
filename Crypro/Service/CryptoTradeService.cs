using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Crypro.Service
{
    public interface ICryptoTradeService
    {
        Task<string> BuyCryptoAsync(CryptoTradeDtoToFunc cryptoTradeDto);
        Task<string> BuyCryptoInit(CryptoTradeDtoToFunc cryptoTradeDto);
        Task<string> SellCryptoAsync(CryptoTradeDtoToFunc cryptoTradeDto);
        Task<string> SellCryptoInit(CryptoTradeDtoToFunc cryptoTradeDto);
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
        public async Task<string> BuyCryptoAsync(CryptoTradeDtoToFunc cryptoTradeDto)
        {
            var fee = await _dbContext.TransactionFees.FirstOrDefaultAsync() ?? throw new Exception("Fee not found");
            var user = _dbContext.Users.Include(x => x.Wallet).FirstOrDefault(x => x.Id.ToString() == cryptoTradeDto.UserId) ?? throw new Exception($"User not found with id: {cryptoTradeDto.UserId}");
            var crypto = _dbContext.Cryptos.FirstOrDefault(x => x.Id.ToString() == cryptoTradeDto.CryptoId) ?? throw new Exception($"Crypto not found with id: {cryptoTradeDto.CryptoId}");
            double totalCost = (cryptoTradeDto.Amount * crypto.value) * (1 + fee.Amount / 100.0);
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
                            IsBuy = Enums.TradeType.Buy,
                        };
                        wallet.Balance -= totalCost;
                        _dbContext.Wallets.Update(wallet);
                        await _dbContext.CryptoPockets.AddAsync(newCryptoPocket);
                        await _dbContext.TradeLogs.AddAsync(tranLog);
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
                            IsBuy = Enums.TradeType.Buy,
                        };
                        wallet.Balance -= totalCost;
                        _dbContext.Wallets.Update(wallet);
                        _dbContext.CryptoPockets.Update(cryptoPocket);
                        await _dbContext.TradeLogs.AddAsync(tranLog);
                        
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

        public async Task<string> SellCryptoAsync(CryptoTradeDtoToFunc cryptoTradeDto)
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
                    double totalCost = cryptoTradeDto.Amount * crypto.value * (1 - fee.Amount / 100.0);
                    cryptoPocket.Amount -= cryptoTradeDto.Amount;
                    var tranLog = new TradeLog
                    {
                        CryptoId = crypto.Id,
                        Amount = cryptoTradeDto.Amount,
                        UserId = user.Id,
                        Date = DateTime.UtcNow,
                        Value = crypto.value,
                        IsBuy = Enums.TradeType.Sell,
                    };
                    wallet.Balance += totalCost;
                    _dbContext.Wallets.Update(wallet);
                    _dbContext.CryptoPockets.Update(cryptoPocket);
                    await _dbContext.TradeLogs.AddAsync(tranLog);
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
        public async Task<string> BuyCryptoInit(CryptoTradeDtoToFunc cryptoTradeDto)
        {
            var user = _dbContext.Users.Include(x => x.Wallet).FirstOrDefault(x => x.Id.ToString() == cryptoTradeDto.UserId) ?? throw new Exception($"User not found with id: {cryptoTradeDto.UserId}");
            var crypto = _dbContext.Cryptos.FirstOrDefault(x => x.Id.ToString() == cryptoTradeDto.CryptoId) ?? throw new Exception($"Crypto not found with id: {cryptoTradeDto.CryptoId}");
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
                        var tranLog = new TradeLog
                        {
                            CryptoId = crypto.Id,
                            Amount = cryptoTradeDto.Amount,
                            UserId = user.Id,
                            Date = DateTime.UtcNow,
                            Value = crypto.value,
                            IsBuy = Enums.TradeType.Buy,
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
                            IsBuy = Enums.TradeType.Buy,
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
        public async Task<string> SellCryptoInit(CryptoTradeDtoToFunc cryptoTradeDto)
        {
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
                    double totalCost = cryptoTradeDto.Amount * crypto.value;
                    cryptoPocket.Amount -= cryptoTradeDto.Amount;
                    var tranLog = new TradeLog
                    {
                        CryptoId = crypto.Id,
                        Amount = cryptoTradeDto.Amount,
                        UserId = user.Id,
                        Date = DateTime.UtcNow,
                        Value = crypto.value,
                        IsBuy = Enums.TradeType.Sell,
                    };
                    wallet.Balance += totalCost;
                    _dbContext.Wallets.Update(wallet);
                    _dbContext.CryptoPockets.Update(cryptoPocket);
                    await _dbContext.TradeLogs.AddAsync(tranLog);
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
