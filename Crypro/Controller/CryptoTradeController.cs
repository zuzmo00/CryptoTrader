using AutoMapper;
using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CryptoTradeController : ControllerBase
    {
        private readonly ICryptoTradeService _cryptoTradeService;
        private readonly IMapper _mapper;
        public CryptoTradeController(ICryptoTradeService cryptoTradeService, IMapper mapper)
        {
            _cryptoTradeService = cryptoTradeService;
            _mapper = mapper;
        }
        /// <summary>
        /// Buy Crypto
        /// </summary>
        /// <param name="cryptoTradeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/trade/buy")]
        public async Task<IActionResult> BuyCrypto([FromBody] CryptoTradeDtoToFunc cryptoTradeDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoTradeService.BuyCrypto(cryptoTradeDto);
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
        [HttpPost]
        [Route("api/trade/sell")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> SellCrypto([FromBody] CryptoTradeDto cryptoTradeDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var id= User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.ToString();
                var data = _mapper.Map<CryptoTradeDtoToFunc>(cryptoTradeDto);
                data.UserId =id;
                var result = await _cryptoTradeService.SellCrypto(data);
                response.Data = result;
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