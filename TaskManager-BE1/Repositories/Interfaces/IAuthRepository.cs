using TaskManager.DTOs;

namespace TaskManager.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApiResponse<UserResponseDto>> Login(string email, string password);
        Task<ApiResponse<string>> Register(RegisterRequestDto registerRequestDto);
    }
}