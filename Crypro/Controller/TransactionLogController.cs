using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionLogController : ControllerBase
    {
        private readonly ITradeLogService _transactionLogService;
        public TransactionLogController(ITradeLogService tradeLogService)
        {
            _transactionLogService = tradeLogService;
        }
        /// <summary>
        /// Get trade log by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/tradelogUser/{userId}")]
        public async Task<IActionResult> GetTradeLog(string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _transactionLogService.GetTransactionByUserId(userId);
                response.Data = data;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }
        }
        /// <summary>
        /// Get trade log by transaction id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/tradelogId/{transactionId}")]
        public async Task<IActionResult> GetTradeLogById(string transactionId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _transactionLogService.GetTransactionById(transactionId);
                response.Data = data;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
