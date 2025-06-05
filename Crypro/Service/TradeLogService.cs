using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface ITradeLogService
    {
        Task<TransactinGetByTransactionId> GetTransactionByIdAsync(string Transactionid);
        Task<List<TransactionGetAllByUserDto>> GetTransactionByUserIdAsync(string userId);

    }
    public class TradeLogService:ITradeLogService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public TradeLogService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TransactinGetByTransactionId> GetTransactionByIdAsync(string Transactionid)
        {
            var transaction = await _dbContext.TradeLogs.FirstOrDefaultAsync(x => x.Id.ToString() == Transactionid) ?? throw new Exception($"Transaction not found with id: {Transactionid}");
            var response = _mapper.Map<TransactinGetByTransactionId>(transaction);
            return response;    
        }

        public async Task<List<TransactionGetAllByUserDto>> GetTransactionByUserIdAsync(string userId)
        {
            var transactions = await _dbContext.TradeLogs.Where(x => x.UserId.ToString() == userId).ToListAsync();
            var response = _mapper.Map<List<TransactionGetAllByUserDto>>(transactions);
            return response;
        }
    }
}
