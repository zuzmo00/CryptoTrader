using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface ILimitdService
    {
        Task<string>LimitBuy(LimitBuyDto limitBuyDto);
        Task<string> LimitSell(LimitSellDto limitSellDto);
        Task<List<LimitGetDto>> ListLimits(Guid Id);
        Task<string> CancelLimit(Guid Id);
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

        public async Task<string> CancelLimit(Guid Id)
        {
            var limit= await _dbContext.LimitedTransactions.FirstOrDefaultAsync(x => x.Id == Id);
             _dbContext.LimitedTransactions.Remove(limit);
            await _dbContext.SaveChangesAsync();
            return "Limit Order Canceled";
        }

        public async Task<string> LimitBuy(LimitBuyDto limitBuyDto)
        {
            var LimitedBuy = _mapper.Map<LimitedTransaction>(limitBuyDto);
            LimitedBuy.Type =TradeType.Buy;
            var log=_mapper.Map<LimitLog>(limitBuyDto);
            log.Type=TradeType.Buy;
            await _dbContext.LimitedTransactions.AddAsync(LimitedBuy);
            await _dbContext.LimitLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
            return "Limit Buy Order Created"; 
        }

        public async Task<string> LimitSell(LimitSellDto limitSellDto)
        {
            var LimitedSell = _mapper.Map<LimitedTransaction>(limitSellDto);
            LimitedSell.Type = TradeType.Sell;
            var log = _mapper.Map<LimitLog>(limitSellDto);
            log.Type = TradeType.Sell;
            await _dbContext.LimitedTransactions.AddAsync(LimitedSell);
            await _dbContext.LimitLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
            return "Limit Sell Order Created";
        }

        public async Task<List<LimitGetDto>> ListLimits(Guid UserId)
        {
            var limits = await _dbContext.LimitedTransactions
                .Where(x => x.UserId == UserId)
                .ToListAsync();
            return _mapper.Map<List<LimitGetDto>>(limits);
            
        }
    }
}
