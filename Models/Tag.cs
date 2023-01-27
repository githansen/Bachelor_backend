using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bachelor_backend.DAL.DatabaseContext;

namespace Bachelor_backend.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        public string TagText { get; set; }
        public virtual List<Text> Texts { get; set; }
    }
}
