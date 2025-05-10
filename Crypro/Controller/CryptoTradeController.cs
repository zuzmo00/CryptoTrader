using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    public class CryptoTradeController : ControllerBase
    {
        private readonly ICryptoTradeService _cryptoTradeService;
        public CryptoTradeController(ICryptoTradeService cryptoTradeService)
        {
            _cryptoTradeService = cryptoTradeService;
        }
        /// <summary>
        /// Buy Crypto
        /// </summary>
        /// <param name="cryptoTradeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/trade/buy")]
        public async Task<IActionResult> BuyCrypto([FromBody] CryptoTradeDto cryptoTradeDto)
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

    }
}