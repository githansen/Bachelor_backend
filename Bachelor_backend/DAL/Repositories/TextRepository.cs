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
                //Check if tag exists in db
                var tagInDb = await _db.Tags.Where(x => x.TagText.Equals(text)).FirstOrDefaultAsync();

                if (tagInDb != null)
                { 
                    _logger.LogInformation("Tag already exists");
                    return false;
                }

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
                        // Creates the Tag if not
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
                return false;
            }

           
            //Add text to db
            try
            {
                string sql = GenerateSql(text);
                TargetGroup Target = await _db.TargetGroups.FromSqlRaw(sql).FirstOrDefaultAsync();
                // --END

                // If it exists, add to Text-object. If not, create it and then add to Text-object
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
                var liste = await _db.Texts.FromSqlRaw($"SELECT dbo.Texts.* FROM dbo.Texts, dbo.TargetGroups WHERE dbo.Texts.TargetGroupTargetid = dbo.TargetGroups.Targetid AND dbo.Texts.Active = 'True' AND (Genders is NULL OR Genders LIKE '%{user.Gender}%') AND (Languages IS NULL OR Languages LIKE '%{user.NativeLanguage}%') AND (Dialects IS NULL OR Dialects LIKE '%{user.Dialect}%') AND (AgeGroups IS NULL OR AgeGroups LIKE '%{user.AgeGroup}%') AND TextId NOT IN (SELECT TextId from dbo.Audiofiles WHERE UserId = '{user.UserId}')"
                   ).Select(t => new Text()
                   {
                       TextId = t.TextId,
                       TextText = t.TextText,
                   }).ToListAsync();

                //If text(s) were found, returns a random one from that list
                if (liste.Count > 0)
                {
                    return GetRandom(liste);
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation(e.Message);
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
                    }).ToListAsync();
                // Returns random from list
                return GetRandom(liste2);
            }
            catch
            {
                return null;
            }
            
        }

      
        public async Task<User> RegisterUserInfo(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(Guid userId)
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

        public async Task<bool> DeleteText(int textId)
        {

            try
            {
                int total = _db.Audiofiles
                .Where(t => t.Text.TextId == textId).Count();
                if (total > 0)
                {
                    return false;
                }
                Text text = await _db.Texts.FindAsync(textId);
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

        public async Task<bool> DeleteTag(int tagId)
        {
            try
            {
                int total =  await _db.Tags.FromSql($"SELECT * FROM dbo.Tags WHERE TagId={tagId} AND TagId IN (SELECT TagsTagId FROM dbo.TagsForTexts)").CountAsync();

                if(total > 0) 
                {
                    return false;
                }
                
                //TODO: Noe forskjell på disse
                //var tag = await _db.Tags.FindAsync(TagId);
                var x = await GetAllTags();
                Tag tag = x.Where(x => x.TagId == tagId).FirstOrDefault();
                if (tag == null || tag?.Texts?.Count > 0)
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
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<int> GetNumberOfTexts()
        {
            try
            {
                int total = await _db.Texts.CountAsync();
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

        public async Task<Text> GetOneText(int textId)
        {
            try
            {
                
                Text text = _db.Texts.Where(t => t.TextId == textId).Select(t => new Text
                {
                    TextId= t.TextId,
                    Tags = t.Tags,
                    TextText = t.TextText,
                    Active = t.Active,
                    TargetGroup = t.TargetGroup
                }).FirstOrDefault();
                return text;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
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
                string sql = GenerateSql(text);

                Text textInDB = await _db.Texts.FindAsync(text.TextId);
                TargetGroup target = await _db.TargetGroups.FromSqlRaw(sql).FirstOrDefaultAsync();
                if(target != null )
                {
                    textInDB.TargetGroup = target;
                }
                else
                {
                    if (textInDB != null)
                    {
                        textInDB.TargetGroup = new TargetGroup()
                        {
                            Genders = text?.TargetGroup?.Genders,
                            Languages = text?.TargetGroup?.Languages,
                            Dialects = text?.TargetGroup?.Dialects,
                            AgeGroups = text?.TargetGroup?.AgeGroups
                        };
                    }
                }

                textInDB.TextText = text.TextText;
                textInDB.Active= text.Active;
                textInDB.Tags = new List<Tag>();
                sql = "DELETE FROM dbo.TagsForTexts WHERE TextsTextId="+text.TextId;
                _db.Database.ExecuteSqlRaw(sql);
                if (text?.Tags != null)
                {
                    foreach (var t in text.Tags)
                    {
                        sql = "INSERT INTO dbo.TagsForTexts (TagsTagId, TextsTextId) VALUES (" + t.TagId + ", + " + text.TextId + ")";
                        _db.Database.ExecuteSqlRaw(sql);
                    }
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                _logger.LogInformation(e.Message);
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> EditTag(Tag tag)
        {
            try
            {
                var tagFromDb = await _db.Tags.FindAsync(tag.TagId);
                if(tagFromDb != null) 
                { 
                    tagFromDb.TagText = tag.TagText;
                    await _db.SaveChangesAsync();
                    return true;
                }
                _logger.LogInformation("Failed to edit tag");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        //Help methods
        public Text GetRandom(List<Text> list)
        {
            Random r = new Random();
            return list[r.Next(0, list.Count)];
        }
        public string GenerateSql(Text text)
        {
            //Check if TargetGroup already exists --START--
            var genders = JsonConvert.SerializeObject(text.TargetGroup?.Genders);
            var languages = JsonConvert.SerializeObject(text.TargetGroup?.Languages);
            var dialects = JsonConvert.SerializeObject(text.TargetGroup?.Dialects);
            var agegroups = JsonConvert.SerializeObject(text.TargetGroup?.AgeGroups);

            string sql = "SELECT * FROM dbo.TargetGroups WHERE";
            if (!genders.ToLower().Equals("null"))
            {
                sql += " Genders= '" + genders + "' AND";
            }
            else
            {
                sql += " Genders IS NULL AND";
            }
            if (!languages.ToLower().Equals("null"))
            {
                sql += " Languages= '" + languages + "' AND";
            }
            else
            {
                sql += " Languages IS NULL AND";
            }
            if (!dialects.ToLower().Equals("null"))
            {
                sql += " Dialects= '" + dialects + "' AND";
            }
            else
            {
                sql += " Dialects IS NULL AND";
            }
            if (!agegroups.ToLower().Equals("null"))
            {
                sql += " AgeGroups= '" + agegroups + "'";
            }
            else
            {
                sql += " AgeGroups IS NULL";
            }

            return sql;
        }

    }
}