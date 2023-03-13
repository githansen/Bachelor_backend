using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories
{
    public interface ISecurityRepository
    {
        Task<bool> Login(AdminUser user);
        Task<bool> Register(AdminUser user);
    }
}