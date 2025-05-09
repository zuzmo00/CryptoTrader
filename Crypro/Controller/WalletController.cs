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
        [Route("{userid}")]
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
                return BadRequest(response);
            }
        }
    }
}
