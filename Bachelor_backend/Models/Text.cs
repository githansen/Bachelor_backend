using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Bachelor_backend.Models
{
    public class Text
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TextId { get; set; }
        
        public string? TextText { get; set; }
        public virtual List<Tag>? Tags { get; set; }

        public TargetGroup? TargetGroup { get; set; }
        public bool Active { get; set; }


      
    }

    // Regex for the targetgroup
   
}
