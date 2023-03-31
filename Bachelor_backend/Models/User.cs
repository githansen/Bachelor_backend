using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        
        [RegularExpression("^([a-zA-ZæøåÆØÅ]{4,16})$")]
        public string? NativeLanguage { get; set; }

        [RegularExpression(@"^(\d){2}-(\d){2}|(\d){2}\+$")]
        public string? AgeGroup { get; set; }
        
        [RegularExpression(@"^([a-zA-ZæøåÆØÅ]{4,16})$")]
        public string? Dialect { get; set; }

        [RegularExpression(@"^(mann|kvinne|annet|Mann|Kvinne|Annet)$")]
        public string? Gender { get; set; }
    }
}
