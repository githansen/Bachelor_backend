using Bachelor_backend.Models;
using Bachelor_backend.Models.APIModels;
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
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> CreateText(SaveText text)
        {
            var TagList = new List<Tag>();
            if(text.TagIds == null || text.TagIds.Count == 0)
            {
                TagList = null;
            }
            else
            {
                foreach (int i in text.TagIds)
                {
                    try
                    {
                        Tag newtag = await _db.Tags.FindAsync(i);
                        TagList.Add(newtag);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            
        

            var newUser = new User()
            {
                Type="TargetUser",
                NativeLanguage = text.NativeLanguage,
                Dialect = text.Dialect,
                AgeGroup = text.AgeGroup,
            };

            if (newUser.NativeLanguage == null && newUser.Dialect == null && newUser.AgeGroup == null) {
                newUser = null;
            }
            try
            {
                var NewText = new Text()
                {
                    TextText = text.TextText,
                    Active= true,
                    Tags= TagList,
                    TargetUser = newUser
                };
                _db.Add(NewText);
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
                    TextId = t.TextId,
                    TextText = t.TextText,
                    Tags = t.Tags.ToList()
                }).ToListAsync();
                return texts;
            }
            catch
            {
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
            var liste2 = await _db.Texts.FromSql($"SELECT * FROM dbo.Texts WHERE dbo.Texts.Active = 1").ToListAsync();
            return getRandom(liste2);
        }

        public Text getRandom(List<Text> list)
        {
            Random r = new Random();
            return list[r.Next(0, list.Count)];

        }
        
        public async Task<User> RegisterUserInfo(User user)
        {
            //TODO: Regex on user items
            user.Type = "RealUser";
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> getUser(int userId)
        {
            try { 
            var user = await _db.Users.FindAsync(userId);
            return user;
            }
            catch { return null; }
               
        }
    }
}
