using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        
        [RegularExpression("^([a-zA-ZæøåÆØÅ]{2,20})$")]
        public string TagText { get; set; }
        public virtual List<Text> Texts { get; set; }
    }
}
