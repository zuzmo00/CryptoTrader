using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfoliController:ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        public PortfoliController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }
        /// <summary>
        /// Get user portfolio
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/portfolio/{userId}")]
        public async Task<IActionResult> GetPortfolio(string userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = await _portfolioService.GetUserPortfolio(userId);
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
