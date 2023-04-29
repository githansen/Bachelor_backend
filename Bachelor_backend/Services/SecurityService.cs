using System.Security.Cryptography;
using Scrypt;

namespace Bachelor_backend.Services
{
    public class SecurityService : ISecurityService
    {
        public string HashPassword(string password, string salt)
        {
            var encoder = new ScryptEncoder();
            var saltedPassword = password + salt;
            return encoder.Encode(saltedPassword);
            
        }
        
        public bool VerifyPassword(string password, string salt, string hash)
        {
            var encoder = new ScryptEncoder();
            var saltedPassword = password + salt;
            return encoder.Compare(saltedPassword, hash);
        }
        
        public string CreateSalt()
        {
            
            var csp = RandomNumberGenerator.Create();
            var salt = new byte[24];
            csp.GetBytes(salt);
            var saltString = Convert.ToBase64String(salt);
            return saltString;
        }
    }
}