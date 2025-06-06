using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvertController:ControllerBase
    {
        private readonly IConvertService _convertService;
        public ConvertController(IConvertService convertService)
        {
            _convertService = convertService;
        }
        [HttpPost]
        [Route("convert")]
        public async Task<IActionResult> ConvertAsync([FromBody] ConvertDto convertDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var result = await _convertService.ConvertAsync(convertDto);
                response.Success = true;
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
