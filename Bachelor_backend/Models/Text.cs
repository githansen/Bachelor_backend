using static Bachelor_backend.DAL.DatabaseContext;

namespace Bachelor_backend.Models
{
    public class Text
    {
        public int TextId { get; set; }
        public string TextText { get; set; }
        public virtual List<TagForText> Tags { get; set; }


    }
}
