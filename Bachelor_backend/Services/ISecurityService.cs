namespace Bachelor_backend.Services
{
    public interface ISecurityService
    {
        string HashPassword(string password, string salt);
        bool VerifyPassword(string password, string salt, string hash);
        string CreateSalt();
    }
}