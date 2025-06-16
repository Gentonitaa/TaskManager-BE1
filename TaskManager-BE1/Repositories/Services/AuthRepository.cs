using Azure.Core;
using Microsoft.AspNetCore.Identity;
using TaskManager.DataContext.Models;
using TaskManager.Repositories.Interfaces;
using TaskManager.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManager.DataContext.Models;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _usermanager;
        private readonly SignInManager<User> _signinmanager;
        private readonly IJwtToken _jwtToken;
        private readonly IUserStore<User> _userStore;



        public AuthRepository(UserManager<User> usermanager, SignInManager<User> signinmanager, IJwtToken jwtToken, IUserStore<User> userStore)
        {
            _usermanager = usermanager;
            _signinmanager = signinmanager;
            _jwtToken = jwtToken;
            _userStore = userStore;
        }

        public async Task<ApiResponse<UserResponseDto>> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return ApiResponseHelper.Error<UserResponseDto>(new List<ApiError> {
                 new ApiError{Field = "email", Message="Email is required"}
                });


            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
                return ApiResponseHelper.Error<UserResponseDto>(new List<ApiError> {
                 new ApiError{Field = "user", Message="User does not exist"}
                });

            var result = await _signinmanager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return ApiResponseHelper.Error<UserResponseDto>(new List<ApiError> {
                 new ApiError{Field = "password", Message="Email or pasword is incorrect"}
                });

            //generate token
            var token = _jwtToken.GenerateToken(user);

            var userResponse = new UserResponseDto
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                Birthdate=user.BirthDate,
                LastName = user.LastName,
                Token = token
            };

            return ApiResponseHelper.Success<UserResponseDto>(userResponse);
        }

        public async Task<ApiResponse<string>> Register(RegisterRequestDto registerRequestDto)
        {
            if (string.IsNullOrWhiteSpace(registerRequestDto.UserName) || string.IsNullOrWhiteSpace(registerRequestDto.Password) || string.IsNullOrEmpty(registerRequestDto.FirstName) || string.IsNullOrEmpty(registerRequestDto.LastName) || (registerRequestDto.Birthdate == DateTime.MinValue))
                return ApiResponseHelper.Error<string>(new List<ApiError> {
                 new ApiError{Field = "Validation", Message="Validation error"}
                });

            var userExists = await _usermanager.FindByEmailAsync(registerRequestDto.UserName);
            if (userExists != null)
                return ApiResponseHelper.Error<string>(new List<ApiError> {
                 new ApiError{Field = "Username", Message="Username already taken"}
                });


            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, registerRequestDto.UserName, CancellationToken.None);
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, registerRequestDto.UserName, CancellationToken.None);
            user.FirstName = registerRequestDto.FirstName;
            user.LastName = registerRequestDto.LastName;
            user.BirthDate = registerRequestDto.Birthdate;
            //save user in db
            var result = await _usermanager.CreateAsync(user, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ApiError
                {
                    Field = e.Code,
                    Message = e.Description
                }).ToList();

                return ApiResponseHelper.Error<string>(errors);
            }
            return ApiResponseHelper.Success<string>(string.Empty);
        }

        //create user
        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor.");
            }
        }
        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_usermanager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}