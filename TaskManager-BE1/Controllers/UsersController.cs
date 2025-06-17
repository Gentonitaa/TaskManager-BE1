using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.DTOs;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           // var authHeader = Request.Headers["Authorization"].ToString();

          //  var token = authHeader.Substring("Bearer ".Length).Trim();
            var result = await _userRepository.ChangePassword(userId, changePasswordDto/*, token*/);
            return Ok(result);

        }
    }
}