using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
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
        [Route("api/limits/buy")]
        public async Task<IActionResult> LimitBuy([FromBody] LimitBuyDto limitBuyDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.LimitBuy(limitBuyDto);
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
        [Route("api/limits/sell")]
        public async Task<IActionResult> LimitSell([FromBody] LimitSellDto limitSellDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.LimitSell(limitSellDto);
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
        [Route("api/limits/{UserId}")]
        public async Task<IActionResult> ListLimits(Guid UserId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.ListLimits(UserId);
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
        [Route("api/limits/{Id}")]
        public async Task<IActionResult> CancelLimit(Guid Id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _limitdService.CancelLimit(Id);
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
