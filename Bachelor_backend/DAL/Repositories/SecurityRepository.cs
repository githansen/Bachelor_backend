using System.Security.Cryptography;
using Bachelor_backend.Models;
using Bachelor_backend.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Scrypt;

namespace Bachelor_backend.DAL.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<SecurityRepository> _logger;

        private readonly ISecurityService _security;

        public SecurityRepository(DatabaseContext db, ILogger<SecurityRepository> logger, ISecurityService security)
        {
            _db = db;
            _logger = logger;
            _security = security;
        }
        
        public async Task<bool> Login(AdminUser user)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Username == user.Username);
            if (admin == null)
            {
                _logger.LogInformation("Wrong username");
                return false;
            }
            var isValid = _security.VerifyPassword(user.Password, admin.Salt, admin.Password);
            
            //Return true if password is valid
            return isValid;
        }

        public async Task<bool> Register(AdminUser user)
        {
            try
            {
                var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Username == user.Username);
                if (admin != null)
                {
                    return false;
                }
                var salt = _security.CreateSalt();
                var hash = _security.HashPassword(user.Password, salt);
                var newAdmin = new AdminUsers()
                {
                    Username = user.Username,
                    Password = hash,
                    Salt = salt
                };
                await _db.Admins.AddAsync(newAdmin);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
            
        }
    }
}