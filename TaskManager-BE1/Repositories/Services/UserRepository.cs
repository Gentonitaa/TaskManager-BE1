using Microsoft.AspNetCore.Identity;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.Repositories.Interfaces;
using TaskManager_BE1.DTOs;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtCheck _jwtCheck;

        public UserRepository(UserManager<User> userManager, JwtCheck jwtCheck)
        {
            _userManager = userManager;
            _jwtCheck = jwtCheck;
        }
        public async Task<ApiResponse<string>> ChangePassword(string userId, ChangePasswordRequestDto changePasswordDto, string token)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ApiResponseHelper.Error<string>(new List<ApiError> {
                 new ApiError{Field = "User", Message="User not found"}
                }, "Password change failed");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ApiError
                {
                    Field = e.Code,
                    Message = e.Description
                }).ToList();

                return ApiResponseHelper.Error<string>(errors, "Password change failed");
            }
            _jwtCheck.Add(token);
            return ApiResponseHelper.Success<string>("Password changed successfully");
        }
    }
}