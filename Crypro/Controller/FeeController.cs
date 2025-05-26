using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    public class FeeController:ControllerBase
    {
        private readonly IFeeService _feeService;

        public FeeController(IFeeService feeService)
        {
            _feeService = feeService;
        }
        /// <summary>
        /// Add fee endpoint
        /// </summary>
        /// <param name="addFeeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/fee")]
        public async Task<IActionResult> AddFee(AddFeeDto addFeeDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _feeService.AddFeeAsync(addFeeDto);
                response.Success = true;
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
        /// Change fee endpoint
        /// </summary>
        /// <param name="changeFeeDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/fee")]
        public async Task<IActionResult> ChangeFee([FromBody] ChangeFeeDto changeFeeDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _feeService.ChangeFeeAsync(changeFeeDto);
                response.Success = true;
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
