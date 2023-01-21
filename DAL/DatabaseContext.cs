using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }


    }
}
