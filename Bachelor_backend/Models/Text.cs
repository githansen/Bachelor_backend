using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class Text
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TextId { get; set; }
        
        [RegularExpression("^([0-9a-zA-ZæøåÆØÅ.'? ]{2,})$")]
        public string TextText { get; set; }
        public virtual List<Tag> Tags { get; set; }

        public TargetUser? TargetUser { get; set; }
        public bool Active { get; set; }
    }
}
