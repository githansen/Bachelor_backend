using static Bachelor_backend.DAL.DatabaseContext;

namespace Bachelor_backend.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagText { get; set; }
        public ICollection<TagForText> texts { get; set; }
    }
}
