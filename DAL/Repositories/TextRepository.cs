using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories
{
    public class TextRepository
    {
        private readonly DatabaseContext _db;
        public TextRepository(DatabaseContext db) {
            _db = db;
        }
        public async Task<List<Text>> GetTexts()
        {
            var liste = _db.Texts.ToList();
            return liste;
        }
        public async Task<List<Tag>> GetTags()
        {
            var liste = _db.Tags.ToList();
            return liste;
        }
        public async Task<bool> CreateText()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> CreateTag()
        {
            throw new NotImplementedException();
        }
    }
}
