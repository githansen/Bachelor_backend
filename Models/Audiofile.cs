using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_backend.Models
{
    public class Audiofile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UUID { get; set; }
        public string path { get; set; }
        public DateTime DateCreated = DateTime.Now;
        public User user { get; set; }

    }
}
