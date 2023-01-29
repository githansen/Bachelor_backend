using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL.Repositories
{
    public class TextRepository : ITextRepository
    {
        private readonly DatabaseContext _db;
        public TextRepository(DatabaseContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateTag(string text)
        {
            try
            {
                var NewTag = new Tag
                {
                    TagText = text,
                    Texts = null

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

        public async Task<Text> GetText(User user)
        {

            var NativeLanguage = user.NativeLanguage;
            var AgeGroup = user.AgeGroup;
            var Dialect = user.Dialect;
            var target = "Target";
            var liste = await _db.Texts.FromSql($"SELECT dbo.Texts.* FROM dbo.Users, dbo.Texts WHERE dbo.Users.UserId = dbo.Texts.UserId AND dbo.Texts.UserId is not NULL AND dbo.Users.Type ={target} AND (dbo.Users.NativeLanguage is NULL or dbo.Users.NativeLanguage={NativeLanguage}) AND (dbo.Users.AgeGroup is NULL or dbo.Users.AgeGroup={AgeGroup}) AND (dbo.Users.Dialect is NULL or dbo.Users.Dialect={Dialect})").ToListAsync();
            //Text out to one user. Not decided yet how this should be done. 
            if (liste.Count > 0) return getRandom(liste);
            var liste2 = await _db.Texts.FromSql($"SELECT * FROM dbo.texts").ToListAsync();
            return getRandom(liste2);
        }

        public Text getRandom(List<Text> list)
        {
            Random r = new Random();
            return list[r.Next(0,list.Count)];
            
        }

        public async Task<User> GetUserInfo(User user)
        {
            //TODO: Regex on user items

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
