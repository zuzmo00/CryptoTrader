using Crypro.DTO;
using Crypro.Entities;
using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _userService.CreateUserAsync(userCreateDto);
                response.Message = "User created successfully";
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
        [Route("Delete/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id,[FromBody] UserUodateDto userUpdateDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _userService.UpdateUserAsync(id, userUpdateDto);
                response.Message = "User updated successfully";
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
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _userService.DeleteUserAsync(id);
                response.Message = "User deleted successfully";
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
        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    return NotFound(response);
                }
                response.Data = user;
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
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _userService.Login(userLoginDto);
                response.Data = token;
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
