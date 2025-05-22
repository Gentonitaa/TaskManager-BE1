namespace TaskManager.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> Login(string email, string password);
    }
}