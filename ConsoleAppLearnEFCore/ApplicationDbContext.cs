using ConsoleAppLearnEFCore.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleAppLearnEFCore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Author> Authors { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=LibraryBookDb;Trusted_Connection=True;");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().Property(b => b.FirstName).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Author>().Property(b => b.LastName).HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Section>().Property(b => b.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Book>().Property(b => b.Name).HasMaxLength(80).IsRequired();

        }
    }
}
