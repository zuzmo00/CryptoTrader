using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LimitContoller:ControllerBase
    {
        private readonly ILimitdService _limitdService;
        public LimitContoller(ILimitdService limitdService)
        {
            _limitdService = limitdService;
        }
        /// <summary>
        /// Create Limit Buy Order
        /// </summary>
        /// <param name="limitBuyDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> LimitBuy([FromBody] LimitBuyDto limitBuyDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.LimitBuyAsync(limitBuyDto);
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
        /// Create Limit Sell Order
        /// </summary>
        /// <param name="limitSellDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sell")]
        public async Task<IActionResult> LimitSell([FromBody] LimitSellDto limitSellDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.LimitSellAsync(limitSellDto);
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
        [HttpGet]
        [Route("{UserId}")]
        public async Task<IActionResult> ListLimits(Guid UserId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.ListLimitsAsync(UserId);
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
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> CancelLimit(Guid Id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.CancelLimitAsync(Id);
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
