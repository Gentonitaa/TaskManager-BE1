using System.Security.Claims;
using TaskManager.DataContext.Models;

namespace TaskManager.Repositories.Interfaces
{
    public interface IJwtToken
    {
        string GenerateToken(User user);
    }
}