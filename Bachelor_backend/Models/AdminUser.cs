using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Bachelor_backend.Models
{
    public class AdminUser
    {
        [Key]
        public String Username { get; set; }
        public String Password { get; set; }
    }
}