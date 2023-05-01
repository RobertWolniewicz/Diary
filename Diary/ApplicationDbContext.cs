using Diary.Models.Configurations;
using Diary.Models.Domains;
using Diary.Properties;
using System;
using System.Data.Entity;
using System.Linq;

namespace Diary
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base($"Server={Settings.Default.Address}\\" +
                  $"{Settings.Default.ServerName};" +
                  $"Database={Settings.Default.DbName};" +
                  $"User Id={Settings.Default.UserName};" +
                  $"Password={Settings.Default.Password};")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
        }
    }
}

