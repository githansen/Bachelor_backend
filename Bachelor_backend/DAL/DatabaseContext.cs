using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bachelor_backend.DAL
{
    public class AdminUsers
    {
        [Key]
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }    


    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Audiofile> Audiofiles { get; set; }
        public DbSet<TargetGroup> TargetGroups { get; set; }
        public DbSet<AdminUsers> Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Text>()
                 .HasMany(t => t.Tags)
                 .WithMany(t => t.Texts)
                 .UsingEntity(t => t.ToTable("TagsForTexts"));


            modelBuilder.Entity<TargetGroup>()
                .Property(x => x.Genders)
                .HasConversion(new ValueConverter<List<string>?, string>(
                   v => JsonConvert.SerializeObject(v), // Convert to string for persistence
            v => JsonConvert.DeserializeObject<List<string>>(v))); // Convert to List<String> for use

            modelBuilder.Entity<TargetGroup>()
               .Property(x => x.AgeGroups)
               .HasConversion(new ValueConverter<List<string>?, string>(
                  v => JsonConvert.SerializeObject(v), // Convert to string for persistence
           v => JsonConvert.DeserializeObject<List<string>>(v))); // Convert to List<String> for use
            modelBuilder.Entity<TargetGroup>()
               .Property(x => x.Dialects)
               .HasConversion(new ValueConverter<List<string>?, string>(
                  v => JsonConvert.SerializeObject(v), // Convert to string for persistence
           v => JsonConvert.DeserializeObject<List<string>>(v))); // Convert to List<String> for use
           
            
            modelBuilder.Entity<TargetGroup>()
               .Property(x => x.Languages)
               .HasConversion(new ValueConverter<List<string>?, string>(
                  v => JsonConvert.SerializeObject(v), // Convert to string for persistence
           v => JsonConvert.DeserializeObject<List<string>>(v))); // Convert to List<String> for use

        }

    }
}
