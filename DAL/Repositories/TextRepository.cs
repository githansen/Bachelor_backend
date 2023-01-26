using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL.Repositories
{
    public class TextRepository : ITextRepository
    {
        private readonly DatabaseContext _db;
        public async Task<bool> CreateTag(string text)
        {
            try
            {
                var NewTag = new Tag
                {
                    TagText = text,
                    texts = null

                };
                _db.Tags.Add(NewTag);
                await _db.SaveChangesAsync();
                  return true;
            } catch {
                return false;
            }
        
        }

        public async Task<bool> CreateText(string text)
        {
            try
            {
                var NewText = new Text { TextText = text };
                _db.Texts.Add(NewText);
                await _db.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<List<Tag>> GetAllTags()
        {
            try
            {
                List<Tag> tags = await _db.Tags.Select(t => new Tag
                {
                    TagId = t.TagId,
                    TagText = t.TagText
                }).ToListAsync();
                return tags;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Text>> GetAllTexts()
        {
            try
            {
                List<Text> texts = await _db.Texts.Select(t => new Text
                {
                    TextId= t.TextId,
                    TextText = t.TextText,
                    Tags = t.Tags.ToList()
                }).ToListAsync();
                return texts;
            }
            catch {
                return null;
            }
        }

        public async Task<Text> GetText()
        {
            //Text out to one user. Not decided yet how this should be done. 
            throw new NotImplementedException();
        }
        public async Task<bool> login()
        {
            throw new NotImplementedException();
        }
    }
}
