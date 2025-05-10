using Crypro.DTO;
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
        /// <summary>
        /// Get crypto by id
        /// </summary>
        /// <param name="cryptoId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cryptos/{cryptoId}")]
        public async Task<IActionResult> GetCryptoById(Guid cryptoId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.GetCryptoById(cryptoId);
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
        /// create crypto
        /// </summary>
        /// <param name="cryptoCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/cryptos")]
        public async Task<IActionResult> CreateCrypto([FromBody] CryptoCreateDto cryptoCreateDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.CryptoCreate(cryptoCreateDto);
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
        /// Delete crypto by id
        /// </summary>
        /// <param name="cryptoId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/cryptos/{cryptoId}")]
        public async Task<IActionResult> RemoveCrypto(Guid cryptoId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.RemoveCrypto(cryptoId);
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
