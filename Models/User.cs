using System.ComponentModel.DataAnnotations;

namespace Bachelor_backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? NativeLanguage { get; set; }
        public string? AgeGroup { get; set; }
        public string? Dialect { get; set; }

    }
}
