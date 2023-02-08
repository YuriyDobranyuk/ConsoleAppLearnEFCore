using ConsoleAppLibrary.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Section = ConsoleAppLibrary.Model.Section;

namespace ConsoleAppLibrary
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateFullDatabase();

        }

        public static void CreateFullDatabase()
        {
            using var dbContext = new ApplicationDbContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        public DbSet<BookSection> BookSections { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                ["Data Source"] = "localhost", // адрес БД к которой подключаемся
                ["Initial Catalog"] = "LibraryDb", // имя БД
                ["Integrated Security"] = true, // используем аутентификацию Windows
                ["Encrypt"] = false // используем аутентификацию Windows
            };*/

            //"Data Source=localhost;Initial Catalog=Achivments;Integrated Security=True;Encrypt=False

            //Console.WriteLine(connectionStringBuilder.ConnectionString);

            // подключаемся к MS SQL Server БД, используя указанную строку подключения
            optionsBuilder
                // настраивает DbContext для подключения к MS SQL Server БД
                .UseSqlServer("Data Source=localhost;Initial Catalog=Achivments;Integrated Security=True;Encrypt=False")
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .LogTo(
                    Console.WriteLine,
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BookAuthor>()
                .HasKey(t => new { t.BookId, t.AuthorId });

            modelBuilder
                .Entity<BookSection>()
                .HasKey(g => new { g.BookId, g.SectionId });

            modelBuilder
                .Entity<Author>()
                .HasMany(t => t.BookAuthors)
                .WithOne(t => t.Author)
                .HasForeignKey(t => t.AuthorId)
                .HasPrincipalKey(t => t.Id);

            modelBuilder
                .Entity<Section>()
                .HasMany(g => g.BookSections)
                .WithOne(g => g.Section)
                .HasForeignKey(g => g.SectionId)
                .HasPrincipalKey(g => g.Id);

            modelBuilder
                .Entity<Book>()
                .HasMany(t => t.BookAuthors)
                .WithOne(t => t.Book)
                .HasForeignKey(t => t.BookId)
                .HasPrincipalKey(t => t.Id);
               
            modelBuilder
                .Entity<Book>()
                .HasMany(t => t.BookSections)
                .WithOne(t => t.Book)
                .HasForeignKey(t => t.BookId)
                .HasPrincipalKey(t => t.Id);

            var csharpBook = new Book
            {
                Id = 1,
                Name = "C# Advanced",
                Description = "Book description for C# Advanced",
                Pages = 452,
                Year = 2019
            };

            var efCoreBook = new Book
            {
                Id = 2,
                Name = "Entity Framework Core Basic",
                Description = "Book description for Entity Framework Core Basic",
                Pages = 452,
                Year = 2019
            };

            var johnSmith = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith"
            };

            var arthurMorgan = new Author
            {
                Id = 2,
                FirstName = "Arthur",
                LastName = "Morgan"
            };

            var sectionClassics = new Section
            {
                Id = 1,
                Name = "Classics",
                Description = "It`s section classic's books."
            };

            var sectionEducation = new Section
            {
                Id = 2,
                Name = "Education",
                Description = "It`s section education's books."
            };

            var sectionFantasy = new Section
            {
                Id = 3,
                Name = "Fantasy",
                Description = "It`s section fantasy's books."
            };

            var connectionsAuthor = new[]
            {
                new BookAuthor
                {
                    BookId = 1,
                    AuthorId = 1,
                },
                new BookAuthor
                {
                    BookId = 1,
                    AuthorId = 2,
                },
                new BookAuthor
                {
                    BookId = 2,
                    AuthorId = 1,
                },
            };

            var connectionsSection = new[]
            {
                new BookSection
                {
                    BookId = 1,
                    SectionId = 1,
                },
                new BookSection
                {
                    BookId = 1,
                    SectionId = 2,
                },
                new BookSection
                {
                    BookId = 2,
                    SectionId = 2,
                },
            };

            // наполняем миграцию данными
            modelBuilder
                .Entity<Book>()
                .HasData(
                    csharpBook,
                    efCoreBook
                );

            modelBuilder
                .Entity<Author>()
                .HasData(
                    johnSmith,
                    arthurMorgan);

            modelBuilder
                .Entity<Section>()
                .HasData(
                    sectionClassics,
                    sectionEducation,
                    sectionFantasy
                );

            modelBuilder
                .Entity<BookAuthor>()
                .HasData(connectionsAuthor);

            modelBuilder
                .Entity<BookSection>()
                .HasData(connectionsSection);
        }
    }
}