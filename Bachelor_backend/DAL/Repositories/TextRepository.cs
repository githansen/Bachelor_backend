using System.Net;
using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Data;
using Newtonsoft.Json;
using System.Globalization;

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
                        else
                        {
                            _db.Tags.Add(tagList[i]);
                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                Debug.Write(e + " TagFeil");
                Console.Write(e + " TagFeil");

                return false;
            }

           
            //Add text to db
            try
            {
                var Target = await _db.TargetUsers
                   .Where(t => t.Genders == text.TargetUser.Genders
                   &&
              t.Languages == text.TargetUser.Languages
                   &&
               t.Dialects == text.TargetUser.Dialects
                   &&
               t.AgeGroups == text.TargetUser.AgeGroups
               ).FirstOrDefaultAsync();
                if (Target != null)
                {
                    text.TargetUser = Target;
                }
                else
                {
                    text.TargetUser = new TargetUser()
                    {
                        Genders = text.TargetUser.Genders,
                        Languages = text.TargetUser.Languages,
                        Dialects = text.TargetUser.Dialects,
                        AgeGroups = text.TargetUser.AgeGroups
                    };
                }
                await _db.Texts.AddAsync(text);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.Write(e + " Textfeil");
                Console.Write(e + " Textfeil");

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
                    TagText = t.TagText
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
                    $"SELECT dbo.Texts.* FROM dbo.Texts, dbo.TargetUsers WHERE Active=1 AND dbo.Texts.TargetUserTargetId = dbo.TargetUsers.TargetId AND dbo.Texts.TextId NOT IN(SELECT dbo.Audiofiles.TextId from dbo.Audiofiles WHERE dbo.Texts.TextId = dbo.Audiofiles.TextId AND dbo.Audiofiles.UserId ={userid}) AND (dbo.TargetUsers.Genders LIKE '%{user.Gender}%') AND (dbo.TargetUsers.Languages LIKE '%{user.NativeLanguage}%') AND (dbo.TargetUsers.Dialects={user.Dialect}) AND (dbo.TargetUsers.AgeGroups={user.AgeGroup}"
                    )
                    .Select(t => new Text
                    {
                        TextId = t.TextId,
                        TextText = t.TextText,

                    }).ToListAsync();
                if (liste.Count > 0)
                {
                    return GetRandom(liste);
                }
            }
            catch
            {
                return null;
            }

            try
            {
                // Gets all active texts
                var liste2 = await _db.Texts.FromSql($"SELECT * FROM dbo.Texts WHERE dbo.Texts.Active = 1 AND dbo.Texts.TextId NOT IN(SELECT dbo.Audiofiles.TextId from dbo.Audiofiles WHERE dbo.Texts.TextId = dbo.Audiofiles.TextId AND dbo.Audiofiles.UserId ={userid})").Select(t =>
                    new Text
                    {
                        TextId = t.TextId,
                        TextText = t.TextText,

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
            //List of age groups
            var ageGroups = new List<string> { "18-29", "30-39", "40-49", "50-59", "60+" };
            
            user.AgeGroup = ageGroups[int.Parse(user.AgeGroup) - 1];
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

        public async Task<int> GetNumberOfTexts()
        {
            try
            {
                int total = await _db.Users.CountAsync();
                return total;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> GetNumberOfUsers()
        {
            try
            {
                int total = await _db.Users.Where(u => u.Type != "Target").CountAsync();
                return total;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<Text> GetOneText(int id)
        {
            try
            {
                
                Text text = _db.Texts.Where(t => t.TextId == id).Select(t => new Text
                {
                    TextId= t.TextId,
                    Tags = t.Tags,
                    TextText = t.TextText,
                    Active = t.Active,
                    TargetUser = t.TargetUser
                }).FirstOrDefault();
                return text;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                List<User> list = await _db.Users.Where(t => t.Type == "RealUser").ToListAsync();
                return list;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> EditText(Text text)
        {
            /* 
             The FindAsync()-method does not get the list of tags since it's fetched from the join table between Texts and Tags
            EF Core 7 does not do this automatically. Also, using Find/FindAsync is necessary for updating the table, 
            therefore we need to get the same object twice -> once to get the tags, and once using the find-method. 

            For the same reason, updates to the tag-list is not automatically updated. Therefore, raw SQL is used to first delete
            all references to the Text we are working on in TagsForTexts table. Then, we insert the rows from the input object (text) 
             */
            try
            {
                Text test = _db.Texts.Where(t => t.TextId == text.TextId).Select(t => new Text
                {
                    Tags = t.Tags,
                }).FirstOrDefault();


                Text textInDB = await _db.Texts.FindAsync(text.TextId);

                var Target = await _db.TargetUsers
                    .Where(t => t.Genders == text.TargetUser.Genders
                    &&
               t.Languages == text.TargetUser.Languages
                    &&
                t.Dialects == text.TargetUser.Dialects
                    &&
                t.AgeGroups == text.TargetUser.AgeGroups
                ).FirstOrDefaultAsync();

                if (Target != null)
                {
                    textInDB.TargetUser = Target;
                }
                else
                {
                    textInDB.TargetUser = new TargetUser()
                    {
                        Genders = text.TargetUser.Genders,
                        Languages = text.TargetUser.Languages,
                        Dialects = text.TargetUser.Dialects,
                        AgeGroups = text.TargetUser.AgeGroups
                    };
                }
                textInDB.TextText = text.TextText;
                textInDB.Active= text.Active;
                textInDB.Tags = new List<Tag>();
                string sql = "DELETE FROM dbo.TagsForTexts WHERE TextsTextId="+text.TextId;
                _db.Database.ExecuteSqlRaw(sql);
                if (text.Tags != null)
                {
                    foreach (Tag t in text.Tags)
                    {
                        sql = "INSERT INTO dbo.TagsForTexts (TagsTagId, TextsTextId) VALUES (" + t.TagId + ", + " + text.TextId + ")";
                        _db.Database.ExecuteSqlRaw(sql);
                    }
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> EditTag(Tag tag)
        {
            try
            {
                Tag TagFromDb = await _db.Tags.FindAsync(tag.TagId);
                TagFromDb.TagText = tag.TagText;
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}