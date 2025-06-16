using TaskManager.DTOs;
using TaskManager.DTOs;

namespace TaskManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<string>> ChangePassword(string userId, ChangePasswordRequestDto changePasswordDto/*, string token*/);
    }
}