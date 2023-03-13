using System.Security.Cryptography;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<SecurityRepository> _logger;

        public SecurityRepository(DatabaseContext db, ILogger<SecurityRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        public async Task<bool> Login(AdminUser user)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Username == user.Username);
            if (admin == null)
            {
                return false;
            }
            var hash = HashPassword(user.Password, admin.Salt);
            if (hash.SequenceEqual(admin.Password))
            {
                return true;
            }
            return false;
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
                var salt = CreateSalt();
                var hash = HashPassword(user.Password, salt);
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

        public static byte[] HashPassword(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 32);
        }
        
        public static byte[] CreateSalt()
        {
            var csp = RandomNumberGenerator.Create();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }
    }
}