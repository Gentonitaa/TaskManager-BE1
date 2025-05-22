using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories.Services
{
    public class AuthRepository : IAuthRepository
    {
        public Task<string> Login(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}