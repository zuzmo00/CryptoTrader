using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public interface IFeeService
    {
        public Task AddFeeAsync(AddFeeDto addFeeDto);
        public Task ChangeFeeAsync(ChangeFeeDto changeFeeDto);
    }
    public class FeeService : IFeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FeeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddFeeAsync(AddFeeDto addFeeDto)
        {
            await _context.TransactionFees.AddAsync(_mapper.Map<TransactionFee>(addFeeDto));
            await _context.SaveChangesAsync();
        }

        public async Task ChangeFeeAsync(ChangeFeeDto changeFeeDto)
        {
            var fee = await _context.TransactionFees.FirstOrDefaultAsync(x => x.Id == changeFeeDto.Id);
            fee.Amount = changeFeeDto.Amount;
            _context.TransactionFees.Update(fee);
            await _context.SaveChangesAsync();

        }
    }
}
