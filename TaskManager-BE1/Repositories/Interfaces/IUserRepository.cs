using TaskManager.DTOs;
using System.Security.Claims;
using TaskManager.DTOs;
using TaskManager.DTOs.UserDto;
namespace TaskManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<string>> ChangePassword(string userId, ChangePasswordRequestDto changePasswordDto/*, string token*/);
    }
}