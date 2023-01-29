using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bachelor_backend.DAL.DatabaseContext;

namespace Bachelor_backend.Models
{
    public class Text
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TextId { get; set; }
        public string TextText { get; set; }
        public virtual List<Tag> Tags { get; set; }

        [ForeignKey("TargetUser")]
        public int? UserId { get; set; }
        public User? TargetUser { get; set; }

    }
}
