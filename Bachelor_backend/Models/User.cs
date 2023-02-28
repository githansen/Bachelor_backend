using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        [RegularExpression("^([a-zA-ZæøåÆØÅ]{4,16})$")]
        public string? NativeLanguage { get; set; }
        public string? AgeGroup { get; set; }
        
        [RegularExpression("^([a-zA-ZæøåÆØÅ]{4,16})$")]
        public string? Dialect { get; set; }
        public string? Gender { get; set; }
    }
}
