using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL
{


    public class DatabaseContext : DbContext
    {
        public class TagForText
        {
            public int TagId { get; set; }
            public Tag tag { get; set; }
            public int TextId { get; set; }
            public Text Text { get; set; }
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Audiofile> Audiofiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           modelBuilder.Entity<Text>()
                .HasMany(t => t.Tags)
                .WithMany(t => t.Texts)
                .UsingEntity(t => t.ToTable("TagsForTexts"));
           
        }
      
    }
}
