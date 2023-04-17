using Bachelor_backend.DBInitializer;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL
{
    public class InitDB
    {
        private readonly DatabaseContext _db;
        private readonly IDBInitializer _dbInitializer;

        public InitDB(DatabaseContext db, IDBInitializer dbInitializer)
        {
            _db = db;
            _dbInitializer = dbInitializer;
        }

        public async Task<bool> initDB()
        {
            try
            {
                _db.Audiofiles.RemoveRange(_db.Audiofiles);
                _db.Tags.RemoveRange(_db.Tags);
                _db.Texts.RemoveRange(_db.Texts);
                _db.Admins.RemoveRange(_db.Admins);
                _db.TargetGroups.RemoveRange(_db.TargetGroups);
                _db.Users.RemoveRange(_db.Users);
                await _db.SaveChangesAsync();
            
                _dbInitializer.Initialize();
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
        }
    }
}