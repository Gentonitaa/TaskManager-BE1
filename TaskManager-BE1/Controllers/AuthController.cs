using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Repositories.Interfaces;
using TaskManager_BE1.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DataContext.Models;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Services;


namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {

        private readonly IAuthRepository _authRepository = authRepository;



        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            return Ok(await _authRepository.Register(request));
        }



        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
        {

            return Ok(await _authRepository.Login(request.UserName, request.Password));

        }

        [HttpGet("GetAuth")]
        [Authorize]
        public IActionResult TestAuth()
        {

            return Ok("Authorized");

        }



    }

}