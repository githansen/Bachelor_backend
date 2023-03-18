

using System.ComponentModel.DataAnnotations;

namespace Bachelor_backend.Models
{
    public class AdminUser
    {
        [RegularExpression(@"^[A-Za-zÆØÅæøå\d._@#$%]{5,30}$")]
        public string Username { get; set; }

        [RegularExpression(@"^[A-Za-zÆØÅæøå\d._@#$%]{5,30}$")]
        public string Password { get; set; }
    }
}