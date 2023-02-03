using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class Audiofile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UUID { get; set; }
        public string? Path { get; set; }
        public DateTime? DateCreated = DateTime.Now;
        public virtual User? User { get; set; }
        public virtual Text? Text { get; set; }

    }
}
