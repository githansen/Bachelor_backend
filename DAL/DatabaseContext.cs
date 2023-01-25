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
            modelBuilder.Entity<TagForText>()
                .HasKey(bc => new { bc.TagId, bc.TextId });
            modelBuilder.Entity<TagForText>()
                .HasOne(bc => bc.Text)
                .WithMany(b => b.Tags)
                .HasForeignKey(bc => bc.TextId);
            modelBuilder.Entity<TagForText>()
                .HasOne(bc => bc.tag)
                .WithMany(b => b.texts)
                .HasForeignKey(bc => bc.TagId);



        }
    }
}
