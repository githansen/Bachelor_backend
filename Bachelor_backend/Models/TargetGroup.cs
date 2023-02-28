using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class TargetGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Targetid { get; set; } 
        public virtual List<string>? Genders { get; set; }
        public virtual List<string>? Languages { get; set; }
        public virtual List<string>? Dialects { get; set; }
        public virtual List<string>? AgeGroups { get; set; }
    }
}
