using AutoMapper;
using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LimitContoller:ControllerBase
    {
        private readonly ILimitdService _limitdService;
        private readonly IMapper _mapper;
        public LimitContoller(ILimitdService limitdService, IMapper mapper)
        {
            _limitdService = limitdService;
            _mapper = mapper;
        }
        /// <summary>
        /// Create Limit Buy Order
        /// </summary>
        /// <param name="limitBuyDtoToController"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> LimitBuy([FromBody] LimitBuyDtoToController limitBuyDtoToController)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.ToString();
                var data = _mapper.Map<LimitBuyDto>(limitBuyDtoToController);
                data.UserId = Guid.Parse(id);
                var responseData = await _limitdService.LimitBuyAsync(data);
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
        /// <param name="limitSellDtoToController"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sell")]
        public async Task<IActionResult> LimitSell([FromBody] LimitSellDtoToController limitSellDtoToController)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var id=User.Claims.First(User=>User.Type==ClaimTypes.NameIdentifier).Value.ToString();
                var data = _mapper.Map<LimitSellDto>(limitSellDtoToController);
                data.UserId = Guid.Parse(id);
                var responseData = await _limitdService.LimitSellAsync(data);
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
