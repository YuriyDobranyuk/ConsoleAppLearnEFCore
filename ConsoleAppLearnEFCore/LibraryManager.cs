using ConsoleAppLearnEFCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppLearnEFCore
{
    public class LibraryManager
    {
        ApplicationDbContext dataBaseLibrary = new ApplicationDbContext();
        List<Section> allSectionsLibrary = null;
        List<Book> allBooksLibrary = null;
        int countBooks;
        int enterNumber;
        
        public void ShowMenuLibrary()
        {
            Console.WriteLine(new String('*', 20));
            Console.WriteLine("Menu library:");
            Console.WriteLine(new String('-', 20));
            Console.WriteLine($"For show library sections, enter number 1.");
            Console.WriteLine($"For find library section, enter number 2.");
            Console.WriteLine($"For add library section, enter number 3.");
            Console.WriteLine($"For edit library section, enter number 4.");
            enterNumber = EnterNumber();
            switch (enterNumber)
            {
                case 1:
                    ShowSectionLibrary();
                    break;
                case 2:
                    FindSectionLibraryByName();
                    break;
                case 3:
                    AddSectionLibraryByName();
                    break;
                case 4:
                    break;
                default:
                    ShowMenuLibrary();
                    break;
            }
        }
        public int EnterNumber()
        {
            Console.WriteLine(new String('*', 10));
            Console.WriteLine($"Enter number, please:");
            var userEnterString = Console.ReadLine();
            var resultConvert = int.TryParse(userEnterString, out enterNumber);
            if (!resultConvert) EnterNumber();
            return enterNumber;
        }
        public void ShowSectionLibrary()
        {
            GetSectionLibrary();
            Console.WriteLine("Section library:");
            foreach (var section in allSectionsLibrary)
            {
                Console.WriteLine($"\tSection Id: {section.Id}");
                Console.WriteLine($"\tName: {section.Name}");
                Console.WriteLine($"\tDescription: {section.Description}");
                Console.WriteLine($"\tCount books: {section.BookSections.Count()}");
                Console.WriteLine("\t" + new String('-', 10));
            }
            ShowMenuLibrary();
        }
        public void GetSectionLibrary()
        {
            allSectionsLibrary = dataBaseLibrary.Sections.Include(section => section.BookSections).ToList();
        }
        public void FindSectionLibraryByName()
        {
            var nameSection = EnterNameSection();
            var librarySection = GetSectionLibraryByName(nameSection);
            if (librarySection != null)
            {
                Console.WriteLine($"Section library by name \"{nameSection}\":");
                Console.WriteLine($"Section Id: {librarySection.Id}");
                Console.WriteLine($"Name: {librarySection.Name}");
                Console.WriteLine($"Description: {librarySection.Description}");
                Console.WriteLine($"Count books: {librarySection.BookSections.Count()}");
                if (librarySection.BookSections.Count() > 0)
                {
                    foreach (var book in librarySection.BookSections)
                    {
                        Console.WriteLine("\t" + new String('&', 10));
                        Console.WriteLine($"\tBook Id: {book.Id}");
                        Console.WriteLine($"\tBook name: {book.Name}");
                        Console.WriteLine($"\tBook description: {book.Description}");
                        Console.WriteLine($"\tCount book Authors: {book.BookAuthors.Count()}");
                        if (book.BookAuthors.Count() > 0)
                        {
                            Console.WriteLine("\t\tBook authors:");
                            Console.WriteLine("\t\t" + new String('-', 10));
                            foreach (var author in book.BookAuthors)
                            {
                                Console.WriteLine($"\t\tAuthor Id: {author.Id}");
                                Console.WriteLine($"\t\tAuthor first name: {author.FirstName}");
                                Console.WriteLine($"\t\tAuthor last name: {author.LastName}");
                                Console.WriteLine("\t\t" + new String('*', 10));
                            }
                        }
                        Console.WriteLine($"\tBook pages: {book.Pages}");
                        Console.WriteLine($"\tBook year: {book.Year}");
                    }
                }
                Console.WriteLine(new String('-', 20));
            }
            else
            {
                Console.WriteLine($"We not find section library by name \"{nameSection}\" in our library.");
            }
            ShowMenuLibrary();
            Console.WriteLine();
        }
        public string EnterNameSection()
        {
            Console.WriteLine(new String('*', 10));
            Console.WriteLine($"Enter name section, please:");
            var nameSection = Console.ReadLine();
            if (nameSection == "") EnterNameSection();
            return nameSection;
        }
        private Section GetSectionLibraryByName(string name)
        {
            var section = dataBaseLibrary.Sections
                                         .Where(section => section.Name == name)
                                         .Include(section => section.BookSections)
                                         .ThenInclude(book => book.BookAuthors)
                                         .FirstOrDefault();
            return section;
        }
        private void AddSectionLibraryByName()
        {
            var nameSection = EnterNameSection();
            var descriptionSection = EnterDescrioptionSection();
            //var sectionBooks = ChooseSectionBooks();
            var sectionBooks = ChooseBooksForSection();
        }
        public string EnterDescrioptionSection()
        {
            Console.WriteLine(new String('*', 10));
            Console.WriteLine($"Enter description section, please:");
            var descriptionSection = Console.ReadLine();
            return descriptionSection;
        }
        public List<Book> ChooseBooksForSection()
        {
            GetBooksLibrary();
            ShowAllBooksLibrary();
            var choosePositionBooksForSection = ChoosePositionSectionBooks();
            var listPositionSectionBooks = MakeListPositionsSectionBooks(choosePositionBooksForSection);
            var choosesBooks = new List<Book>();
            var i = 1;
            /*foreach (var position in listPositionSectionBooks)
            {

            }*/
            return choosesBooks;
        }
        
        public void GetBooksLibrary()
        {
            allBooksLibrary = dataBaseLibrary.Books
                                             .Include(book => book.BookAuthors)
                                             .Include(book => book.BookSections)
                                             .ToList();
        }
        public void ShowAllBooksLibrary()
        {
            countBooks = allBooksLibrary.Count();
            Console.WriteLine($"Book library:");
            
            Console.WriteLine($"Count books: {countBooks}");
            if (countBooks > 0)
            {
                var i = 0;
                foreach (var book in allBooksLibrary)
                {
                    Console.WriteLine(new String('&', 10));
                    Console.WriteLine($"Number book: \"{i}\"");
                    Console.WriteLine($"Book name: {book.Name}");
                    Console.WriteLine($"Book description: {book.Description}");
                    Console.WriteLine($"Count book Authors: {book.BookAuthors.Count()}");
                    if (book.BookAuthors.Count() > 0)
                    {
                        Console.WriteLine("\tBook authors:");
                        Console.WriteLine("\t" + new String('-', 10));
                        var j = 1;
                        foreach (var author in book.BookAuthors)
                        {
                            Console.WriteLine($"\t{j}. {author.LastName} {author.FirstName}");
                            j++;
                        }
                    }
                    Console.WriteLine($"Book pages: {book.Pages}");
                    Console.WriteLine($"Book year: {book.Year}");
                    Console.WriteLine($"Count book sections: {book.BookSections.Count()}");
                    if (book.BookSections.Count() > 0)
                    {
                        Console.WriteLine("\tBook sections:");
                        Console.WriteLine("\t" + new String('-', 10));
                        var k = 1;
                        foreach (var section in book.BookSections)
                        {
                            Console.WriteLine($"\t{k}) {section.Name} ({section.Description})");
                            Console.WriteLine("\t" + new String('*', 10));
                            k++;
                        }
                    }
                    i++;
                }
            }
            Console.WriteLine(new String('-', 20));
        }
        public string ChoosePositionSectionBooks()
        {
            Console.WriteLine(new String('*', 10));
            Console.WriteLine($"Enter the book number separated by a comma, please:");
            var sectionBooksString = Console.ReadLine();
            return sectionBooksString;
        }
        public List<int> MakeListPositionsSectionBooks(string positionString)
        {
            var arrayPositionSectionBooks = positionString.Split(new char[] { ',' });
            var listPositionSectionBooks = new List<int>();
            var num = 0;
            foreach (var positionBook in arrayPositionSectionBooks)
            {
                if (int.TryParse(positionBook, out num) && num > 0 && num <= )
                {
                    listPositionSectionBooks.Add(num);
                }
            }
            return listPositionSectionBooks;
        }


    }
}
