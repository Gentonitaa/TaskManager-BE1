using System.Security.Claims;

namespace TaskManager.Repositories.Interfaces
{
    public interface IJwtToken
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}