using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Route("list")]
        public async Task<IActionResult> ListCryptos()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.ListCryptosAsync();
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
        [Route("{cryptoId}")]
        public async Task<IActionResult> GetCryptoById(Guid cryptoId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.GetCryptoByIdAsync(cryptoId);
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
        [Route("create")]
        public async Task<IActionResult> CreateCrypto([FromBody] CryptoCreateDto cryptoCreateDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.CryptoCreateAsync(cryptoCreateDto);
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
        [Route("{cryptoId}")]
        public async Task<IActionResult> RemoveCrypto(Guid cryptoId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.RemoveCryptoAsync(cryptoId);
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
        /// Update crypto price
        /// </summary>
        /// <param name="cryptoUpdateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("price")]
        public async Task<IActionResult> UpdateCrypto([FromBody] CryptoUpdateDto cryptoUpdateDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _cryptoManagerService.UpdateCryptoAsync(cryptoUpdateDto);
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
