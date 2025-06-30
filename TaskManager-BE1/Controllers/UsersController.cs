using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.DataContext.Models;
using TaskManager.DTOs.UserDto;
using TaskManager.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.DTOs.UserDto;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Services;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        // ********************** CHANGE PASSWORD ******************************
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var authHeader = Request.Headers["Authorization"].ToString();

            //    var token = authHeader.Substring("Bearer ".Length).Trim();

            var result = await _userRepository.ChangePassword(userId, changePasswordDto/*, token*/);
            return Ok(result);
        }

        // ********************** GET ALL USERS ******************************
        [HttpGet("users")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userRepository.GetAllUsersAsync();

            if (!result.Status)
                return NotFound(result);

            return Ok(result);
        }
    }
}