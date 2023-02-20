using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [RegularExpression("^()$")] //User is not allowed to change type
        public string? Type { get; set; }//TargetUser and RealUser
        
        [RegularExpression("^([a-zA-ZæøåÆØÅ]{4,16})$")]
        public string? NativeLanguage { get; set; }
        
        [RegularExpression("^([0-9]{2}-[0-9]{2})|([60+])$")]
        public string? AgeGroup { get; set; }
        public string? Dialect { get; set; }

    }
}
