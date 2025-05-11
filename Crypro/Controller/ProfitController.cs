using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    public class ProfitController:ControllerBase
    {
        private readonly IProfitService _profitService;
        public ProfitController(IProfitService profitService)
        {
            _profitService = profitService;
        }
        /// <summary>
        /// All profit endpoint
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/profit/{userId}")]
        public async Task<IActionResult> GetProfit(string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _profitService.GetUserProfit(userId);
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
        /// Get profit by crypto endpoint
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/profit/details/{userId}")]
        public async Task<IActionResult> GetProfitDetails(string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _profitService.GetProfitByCrypto(userId);
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
