using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL.Repositories
{
    public class TextRepository : ITextRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<TextRepository> _logger;

        public TextRepository(DatabaseContext db, ILogger<TextRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> CreateTag(string text)
        {
            try
            {
                var NewTag = new Tag
                {
                    TagText = text,
                    Texts = new List<Text>()
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

        public async Task<bool> CreateText(Text text)
        {
            try
            {
                //Creates tags if they don't exist
                var tagList = text.Tags;
                if (tagList != null)
                {
                    for (int i = 0; i < tagList.Count(); i++)
                    {
                        //Check if tag exists in db
                        var tagInDb = await _db.Tags.Where(x => x.TagText.Equals(tagList[i].TagText)).FirstOrDefaultAsync();

                        // Gives the tag an id if it exists
                        if (tagInDb != null)
                        {
                            tagList[i] = tagInDb;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }

            //Add text to db
            try
            {
                var user = text.TargetUser;
                user.Type = "TargetUser";
                await _db.Users.AddAsync(user);
                await _db.Texts.AddAsync(text);

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<List<Tag>> GetAllTags()
        {
            try
            {
                List<Tag> tags = await _db.Tags.Select(t => new Tag
                {
                    TagId = t.TagId,
                    TagText = t.TagText,
                    Texts = t.Texts.ToList()
                }).ToListAsync();
                return tags;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
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
                    Tags = t.Tags.ToList(),
                    Active = t.Active,
                    TargetUser = t.TargetUser
                }).ToListAsync();

                return texts;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<Text> GetText(User user)
        {
            var NativeLanguage = user.NativeLanguage;
            var AgeGroup = user.AgeGroup;
            var Dialect = user.Dialect;
            var target = "Target";
            var userid = user.UserId;
            // Finds lists of texts with a target group that fits the user requesting text
            try
            {
                var liste = await _db.Texts
                    .FromSql(
                        $"SELECT dbo.Texts.* FROM dbo.Users, dbo.Texts WHERE dbo.Texts.Active=1 AND dbo.Users.UserId = dbo.Texts.UserId AND dbo.Texts.UserId is not NULL AND dbo.Users.Type ={target} AND (dbo.Users.NativeLanguage is NULL or dbo.Users.NativeLanguage={NativeLanguage}) AND (dbo.Users.AgeGroup is NULL or dbo.Users.AgeGroup={AgeGroup}) AND (dbo.Users.Dialect is NULL or dbo.Users.Dialect={Dialect}) AND dbo.Texts.TextId NOT IN(SELECT dbo.Audiofiles.TextId from dbo.Audiofiles, dbo.Texts WHERE dbo.Texts.TextId = dbo.Audiofiles.TextId AND dbo.Audiofiles.UserId ={userid})")
                    .Select(t => new Text
                    {
                        TextId = t.TextId,
                        TextText = t.TextText,
                        Tags = t.Tags.ToList(),
                        Active = t.Active,
                        TargetUser = t.TargetUser
                    }).ToListAsync();
                if (liste.Count > 0) return GetRandom(liste);
            }
            catch
            {
                return null;
            }

            try
            {
                // Gets all active texts
                var liste2 = await _db.Texts.FromSql($"SELECT * FROM dbo.Texts WHERE dbo.Texts.Active = 1 AND dbo.Texts.TextId NOT IN(SELECT dbo.Audiofiles.TextId from dbo.Audiofiles, dbo.Texts WHERE dbo.Texts.TextId = dbo.Audiofiles.TextId AND dbo.Audiofiles.UserId ={userid})").Select(t =>
                    new Text
                    {
                        TextId = t.TextId,
                        TextText = t.TextText,
                        Tags = t.Tags.ToList(),
                        Active = t.Active,
                        TargetUser = t.TargetUser
                    }).ToListAsync();
                return GetRandom(liste2);
            }
            catch
            {
                return null;
            }
        }

        public Text GetRandom(List<Text> list)
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

        public async Task<User> GetUser(int userId)
        {
            try
            {
                var user = await _db.Users.FindAsync(userId);
                return user;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> DeleteText(int TextId)
        {
            var textInAudioFile = _db.Audiofiles
                .FromSql($"SELECT * from dbo.Audiofiles WHERE dbo.Audiofiles.UserId ={TextId}").ToList();
            if (textInAudioFile.Count > 0)
            {
                return false;
            }

            try
            {
                Text text = await _db.Texts.FindAsync(TextId);
                _db.Texts.Remove(text);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteTag(int TagId)
        {
            try
            {
                var x = await GetAllTags();
                Tag tag = x.Where(i => i.TagId == TagId).FirstOrDefault();
                if (tag == null || tag.Texts.Count > 0)
                {
                    return false;
                }

                _db.Tags.Remove(tag);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }
    }
}