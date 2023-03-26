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
                
                await _db.Tags.ExecuteDeleteAsync();
                await _db.Texts.ExecuteDeleteAsync();
                await _db.Admins.ExecuteDeleteAsync();
                await _db.TargetGroups.ExecuteDeleteAsync();
                await _db.Audiofiles.ExecuteDeleteAsync();
                await _db.Users.ExecuteDeleteAsync();
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