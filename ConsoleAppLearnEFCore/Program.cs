using ConsoleAppLearnEFCore.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleAppLearnEFCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // наполняем миграцию данными
                var csharpBook = new Book
                {
                    Name = "C# Advanced",
                    Description = "Book description for C# Advanced",
                    Pages = 452,
                    Year = 2019
                };

                var efCoreBook = new Book
                {
                    Name = "Entity Framework Core Basic",
                    Description = "Book description for Entity Framework Core Basic",
                    Pages = 452,
                    Year = 2019
                };

                db.Books.AddRange(csharpBook, efCoreBook);

                var johnSmith = new Author
                {
                    FirstName = "John",
                    LastName = "Smith"
                };

                var arthurMorgan = new Author
                {
                    FirstName = "Arthur",
                    LastName = "Morgan"
                };

                db.Authors.AddRange(johnSmith, arthurMorgan);

                var sectionClassics = new Section
                {
                    Name = "Classics",
                    Description = "It`s section classic's books."
                };

                var sectionEducation = new Section
                {
                    Name = "Education",
                    Description = "It`s section education's books."
                };

                var sectionFantasy = new Section
                {
                    Name = "Fantasy",
                    Description = "It`s section fantasy's books."
                };

                db.Sections.AddRange(sectionClassics, sectionEducation, sectionFantasy);

                csharpBook.BookAuthors.Add(johnSmith);
                csharpBook.BookAuthors.Add(arthurMorgan); 
                csharpBook.BookSections.Add(sectionClassics);
                csharpBook.BookSections.Add(sectionEducation);

                efCoreBook.BookAuthors.Add(johnSmith);
                efCoreBook.BookSections.Add(sectionEducation);
                
                db.SaveChanges();
            }
        }

        
    }
    

    

}