using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface ILimitdService
    {
        Task<string>LimitBuyAsync(LimitBuyDto limitBuyDto);
        Task<string> LimitSellAsync(LimitSellDto limitSellDto);
        Task<List<LimitGetDto>> ListLimitsAsync(Guid Id);
        Task<string> CancelLimitAsync(Guid Id);
    }
    public class LimitedService : ILimitdService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public LimitedService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string> CancelLimitAsync(Guid Id)
        {
            var limit= await _dbContext.LimitedTransactions.FirstOrDefaultAsync(x => x.Id == Id)?? throw new Exception("Limit not found");
             _dbContext.LimitedTransactions.Remove(limit);
            await _dbContext.SaveChangesAsync();
            return "Limit Order Canceled";
        }

        public async Task<string> LimitBuyAsync(LimitBuyDto limitBuyDto)
        {
            var wallet= await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == limitBuyDto.UserId) ?? throw new Exception("User not found");
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id == limitBuyDto.CryptoId) ?? throw new Exception("Crypto not found");
            var price= limitBuyDto.Limit * limitBuyDto.Amount;
            if (wallet.Balance < price)
            {
                throw new Exception("Not enough balance");
            }
            var LimitedBuy = _mapper.Map<LimitedTransaction>(limitBuyDto);
            LimitedBuy.Type =Enums.TradeType.Buy;
            var log=_mapper.Map<LimitLog>(limitBuyDto);
            log.Type=Enums.TradeType.Buy;
            await _dbContext.LimitedTransactions.AddAsync(LimitedBuy);
            await _dbContext.LimitLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
            return "Limit Buy Order Created"; 
        }

        public async Task<string> LimitSellAsync(LimitSellDto limitSellDto)
        {
            var wallet = await _dbContext.Wallets
                .Include(x => x.CryptoPockets)
                .FirstOrDefaultAsync(x => x.UserId == limitSellDto.UserId) ?? throw new Exception("User not found");
            if(wallet.CryptoPockets == null)
            {
                throw new Exception("No crypto found");
            }
            var crypto = await _dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id == limitSellDto.CryptoId) ?? throw new Exception("Crypto not found");
            if(limitSellDto.Amount > wallet.CryptoPockets.FirstOrDefault(x => x.CryptoId == limitSellDto.CryptoId).Amount)
            {
                throw new Exception("Not enough crypto");
            }
            var LimitedSell = _mapper.Map<LimitedTransaction>(limitSellDto);
            LimitedSell.Type = Enums.TradeType.Sell;
            var log = _mapper.Map<LimitLog>(limitSellDto);
            log.Type = Enums.TradeType.Sell;
            await _dbContext.LimitedTransactions.AddAsync(LimitedSell);
            await _dbContext.LimitLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
            return "Limit Sell Order Created";
        }

        public async Task<List<LimitGetDto>> ListLimitsAsync(Guid UserId)
        {
            var limits = await _dbContext.LimitedTransactions
                .Where(x => x.UserId == UserId)
                .ToListAsync();
            return _mapper.Map<List<LimitGetDto>>(limits);
            
        }
    }
}
