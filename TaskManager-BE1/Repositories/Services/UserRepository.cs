using Microsoft.AspNetCore.Identity;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.DTOs.UserDto;
using TaskManager.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.DTOs.IssueDto;
using TaskManager.DTOs.UserDto;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        //private readonly JwtCheck _jwtCheck;

        public UserRepository(UserManager<User> userManager/*, JwtCheck jwtCheck */, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
            //_jwtCheck = jwtCheck;
        }

        // ********************** CHANGE PASSWORD ******************************
        public async Task<ApiResponse<string>> ChangePassword(string userId, ChangePasswordRequestDto changePasswordDto/*, string token*/)
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
            await _userManager.UpdateSecurityStampAsync(user);
            //_jwtCheck.Add(token);
            return ApiResponseHelper.Success<string>("Password changed successfully");
        }


        // ********************** GET ALL USERS ******************************
        public async Task<ApiResponse<List<GetAllUsersDto>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();

            var userList = users.Select(user => new GetAllUsersDto
            {
                Id = user.Id,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).ToList();

            return ApiResponseHelper.Success(userList);
        }
      


        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

    }
}
