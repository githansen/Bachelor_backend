using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories
{
    public interface ISecurityRepository
    {
        Task<bool> Login(AdminUser user);
        Task<bool> FindAdmin(string username);
        Task<bool> Register(AdminUser user);
    }
}