using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        [HttpGet]
        [Route("/api/wallet/{userId}")]
        public async Task<IActionResult> GetWallet(string userid)
        {
            ApiResponse response=new ApiResponse();
            try
            {
                var data = await _walletService.GetWallet(userid);
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
        [HttpPut]
        [Route("/api/wallet/{userId}")]
        public async Task<IActionResult> AddToBalance(Guid userId, AddToBalanceDto addToBalanceDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _walletService.AddToBalance(userId, addToBalanceDto);
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
        [Route("api/wallet/{userId}")]
        public async Task<IActionResult> DeleteWallet(Guid userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _walletService.DeleteWallet(userId);
                response.Data = data;
                response.Message = "Wallet deleted successfully";
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
