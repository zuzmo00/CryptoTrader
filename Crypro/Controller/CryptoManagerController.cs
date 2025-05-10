using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    public class CryptoManagerController: ControllerBase
    {
        private readonly ICryptoManagerService _cryptoManagerService;
        public CryptoManagerController(ICryptoManagerService cryptoManagerService)
        {
            _cryptoManagerService = cryptoManagerService;
        }
        /// <summary>
        /// List all cryptos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cryptos)")]
        public async Task<IActionResult> ListCryptos()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.ListCryptos();
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
