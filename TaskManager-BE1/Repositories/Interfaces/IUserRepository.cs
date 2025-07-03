using System.Security.Claims;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.DTOs;
using TaskManager.DTOs.UserDto;
namespace TaskManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<string>> ChangePassword(string userId, ChangePasswordRequestDto changePasswordDto/*, string token*/);
        Task<ApiResponse<List<GetAllUsersDto>>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid userId);


    }
}