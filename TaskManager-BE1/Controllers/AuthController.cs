using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.DTOs.UserDto;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _usermanager;
        private readonly SignInManager<User> _signinmanager;
        private readonly IJwtToken _jwtToken;
        private readonly IUserStore<User> _userStore;
        private readonly IAuthRepository _authRepository;

        public AuthController(
            UserManager<User> usermanager,
            SignInManager<User> signinmanager,
            IJwtToken jwtToken,
            IUserStore<User> userStore,
            IAuthRepository authRepository)
        {
            _usermanager = usermanager;
            _signinmanager = signinmanager;
            _jwtToken = jwtToken;
            _userStore = userStore;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            return Ok(await _authRepository.Register(request));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ApiResponse<UserResponseDto>> Login([FromBody] LoginRequestDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return ApiResponseHelper.Error<UserResponseDto>(new List<ApiError> {
            new ApiError { Field = "email", Message = "Email is required" }
        });
            }

            var user = await _usermanager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ApiResponseHelper.Error<UserResponseDto>(new List<ApiError> {
            new ApiError { Field = "user", Message = "User does not exist" }
        });
            }

            var result = await _signinmanager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return ApiResponseHelper.Error<UserResponseDto>(new List<ApiError> {
            new ApiError { Field = "password", Message = "Email or password is incorrect" }
        });
            }

            user.LastLogin = DateTime.UtcNow;
            await _usermanager.UpdateAsync(user);

            var roles = (await _usermanager.GetRolesAsync(user)).ToList();
            var token = _jwtToken.GenerateToken(user);

            var userResponse = new UserResponseDto
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.BirthDate,
                Token = token,
                Roles = roles,
                LastLogin = user.LastLogin
            };

            return ApiResponseHelper.Success<UserResponseDto>(userResponse);
        }



        [HttpGet("GetAuth")]
        public IActionResult TestAuth()
        {
            return Ok("Authorized");
        }
    }
}