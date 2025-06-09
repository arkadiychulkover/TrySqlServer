using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrySqlServer.Entity;

namespace TrySqlServer
{
    public class AppDBContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Viever2> Viever { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=EfDemoDb;Integrated Security=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users").HasKey(s => s.Id);
                e.HasIndex(s => s.Username).IsUnique();
                e.HasIndex(s => s.Email).IsUnique();
                e.ToTable(t => t.HasCheckConstraint("CK_Users_Email", "[Email] Like '%@%.%'"));
            });

            modelBuilder.Entity<Movie>(e =>
            {
                e.ToTable("Movie").HasKey(s => s.Id);
                e.Property(s => s.Title).HasMaxLength(50);
                e.ToTable(t => t.HasCheckConstraint("CK_Student_Email", "[ReleaseYear] > 0"));
            });


            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Users)
                .WithMany(u => u.Movies)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Viever2>()
                .HasNoKey()
                .ToView("UserMoviesView");


        }
    }
}
