using Bachelor_backend.DAL;
using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Services;

namespace Bachelor_backend.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly DatabaseContext _db;
        private readonly ISecurityService _security;
        public DBInitializer(DatabaseContext db, ISecurityService security) 
        {
            _db = db;
            _security = security;
        }
        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }       
            
            //Add admin user if none exists
            if (!_db.Admins.Any())
            {
                var salt = _security.CreateSalt();
                var hash = _security.HashPassword("admin", salt);
                var admin = new AdminUsers()
                {
                    Username = "admin",
                    Password = hash,
                    Salt = salt
                };
            
                _db.Admins.Add(admin);
                _db.SaveChanges();
            }
            
            
            if(_db.Texts.Count() > 0) { return; }



            string CurrentDirectory = Directory.GetCurrentDirectory();
            Console.Write(Directory.GetCurrentDirectory());
            StreamReader reader = null;
            string filepathTags = System.IO.Path.Combine(CurrentDirectory, @"TestData\Tags.txt");
            Console.WriteLine(filepathTags);
            reader = new StreamReader(filepathTags);
            var Taglist = new List<Tag>();
            
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var tag = new Tag()
                {
                    TagText = line
                };
                Taglist.Add(tag);
                _db.Tags.Add(tag);
                _db.SaveChanges();
            }
            TargetGroup t = new TargetGroup()
            {
                Genders = new List<string> { "Mann", "Kvinne" },
                AgeGroups = new List<string> { "18-29", "29-38"}
            };
            string filepathTexts = System.IO.Path.Combine(CurrentDirectory, @"TestData\Texts.txt"); ;
            
            reader = new StreamReader(filepathTexts);
            while (!reader.EndOfStream)
                {
                    Random r = new Random();
                
                    var line = reader.ReadLine();
                var text = new Text()
                {
                    TextText = line,
                    Active = true,
                    Tags = new List<Tag> { Taglist[r.Next(0, Taglist.Count)], Taglist[r.Next(0, Taglist.Count)] },
                    };
                if(r.Next(0,2) == 1)
                {
                    text.TargetGroup = t;
                }
                _db.Texts.Add(text);
                _db.SaveChanges();
                }
          
            string filepathUsers = System.IO.Path.Combine(CurrentDirectory, @"TestData\Users.txt");
            reader = new StreamReader(filepathUsers);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var user = new User()
                {
                    NativeLanguage= line.Split()[1],
                    AgeGroup = line.Split()[0],
                };
                if (Equals(user.NativeLanguage, "Norsk"))
                {
                    user.Dialect = line.Split()[2];
                    user.Gender = line.Split()[3];
                }
                else
                {
                    user.Gender = line.Split()[2];
                }
                _db.Users.Add(user);
                _db.SaveChanges();
            }

        }
    }
}
