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
                var Target = await _db.TargetGroups
                   .Where(t => t.Genders == text.TargetGroup.Genders
                   &&
              t.Languages == text.TargetGroup.Languages
                   &&
               t.Dialects == text.TargetGroup.Dialects
                   &&
               t.AgeGroups == text.TargetGroup.AgeGroups
               ).FirstOrDefaultAsync();
                if (Target != null)
                {
                    text.TargetGroup = Target;
                }
                else
                {
                    text.TargetGroup = new TargetGroup()
                    {
                        Genders = text.TargetGroup.Genders,
                        Languages = text.TargetGroup.Languages,
                        Dialects = text.TargetGroup.Dialects,
                        AgeGroups = text.TargetGroup.AgeGroups
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
                    TargetGroup = t.TargetGroup
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

            // Finds lists of texts with a target group that fits the user requesting text
            try
            {

                Debug.WriteLine($"SELECT dbo.Texts.* FROM dbo.Texts, dbo.TargetGroups WHERE dbo.Texts.TargetUserTargetid = dbo.TargetGroups.Targetid AND dbo.Texts.Active = 'True' AND (Genders is NULL OR Genders LIKE '%{user.Gender}%') AND (Languages IS NULL OR Languages LIKE '%{user.NativeLanguage}%') AND (Dialects IS NULL OR Dialects LIKE '%{user.Dialect}%') AND (AgeGroups IS NULL OR AgeGroups LIKE '%{user.AgeGroup}%')");
                var l = await _db.Texts.FromSqlRaw($"SELECT dbo.Texts.* FROM dbo.Texts, dbo.TargetGroups WHERE dbo.Texts.TargetGroupTargetid = dbo.TargetGroups.Targetid AND dbo.Texts.Active = 'True' AND (Genders is NULL OR Genders LIKE '%{user.Gender}%') AND (Languages IS NULL OR Languages LIKE '%{user.NativeLanguage}%') AND (Dialects IS NULL OR Dialects LIKE '%{user.Dialect}%') AND (AgeGroups IS NULL OR AgeGroups LIKE '%{user.AgeGroup}%')"
                   ).Select(t => new Text()
                   {
                       TextId = t.TextId,
                       TextText = t.TextText,
                       TargetGroup = t.TargetGroup
                   }).ToListAsync();
                if (l.Count > 0)
                {
                    return GetRandom(l);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }

            try
            {
                // Gets all active texts
                var liste2 = await _db.Texts.Select(t => 
                    new Text
                    {
                        TextId = t.TextId,
                        TextText = t.TextText,
                        Active = t.Active,
                        TargetGroup = t.TargetGroup
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
                int total = await _db.Users.CountAsync();
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
                    TargetGroup = t.TargetGroup
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
                List<User> list = await _db.Users.ToListAsync();
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

                var Target = await _db.TargetGroups
                    .Where(t => t.Genders == text.TargetGroup.Genders
                    &&
               t.Languages == text.TargetGroup.Languages
                    &&
                t.Dialects == text.TargetGroup.Dialects
                    &&
                t.AgeGroups == text.TargetGroup.AgeGroups
                ).FirstOrDefaultAsync();

                if (Target != null)
                {
                    textInDB.TargetGroup = Target;
                }
                else
                {
                    textInDB.TargetGroup = new TargetGroup()
                    {
                        Genders = text.TargetGroup.Genders,
                        Languages = text.TargetGroup.Languages,
                        Dialects = text.TargetGroup.Dialects,
                        AgeGroups = text.TargetGroup.AgeGroups
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