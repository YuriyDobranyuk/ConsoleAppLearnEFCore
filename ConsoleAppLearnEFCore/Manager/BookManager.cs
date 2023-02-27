using ConsoleAppLearnEFCore.Model;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAppLearnEFCore.Manager
{
    public class BookManager
    {
        ApplicationDbContext dataBaseLibrary = new ApplicationDbContext();
        
        int enterNumber;
        string enterNameBook;
        Book findBook;
        int countBooks;

        public List<Book>? Books { get; set; }
        public SectionManager SectionManager { get; }
        public AuthorManager AuthorManager { get; }

        public void ShowMenuBookLibrary()
        {
            Console.WriteLine(new string('*', 20));
            Console.WriteLine("Menu library books:");
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"For show all books, enter number 1.");
            Console.WriteLine($"For find book, enter number 2.");
            Console.WriteLine($"For add book to library, enter number 3.");
            Console.WriteLine($"For edit book, enter number 4.");
            Console.WriteLine($"For return to menu library, enter number 0.");
            enterNumber = EnterNumber();
            switch (enterNumber)
            {
                case 1:
                    ShowAllBooksLibrary();
                    break;
                case 2:
                    SearchBookLibraryByName();
                    break;
                case 3:
                    AddBookLibraryByName();
                    break;
                case 4:
                    Edit();
                    break;
            }
            Console.WriteLine();
            if (enterNumber != 0) ShowMenuBookLibrary();
        }
        private int EnterNumber()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter number, please:");
            var userEnterString = Console.ReadLine();
            var resultConvert = int.TryParse(userEnterString, out enterNumber);
            if (!resultConvert) EnterNumber();
            return enterNumber;
        }
        public void ShowAllBooksLibrary()
        {
            GetBooksLibrary();

            countBooks = Books.Count();
            Console.WriteLine($"Books library:");
            Console.WriteLine($"Count books: {countBooks}");
            if (countBooks > 0)
            {
                var i = 1;
                foreach (var book in Books)
                {
                    ShowBook(book, i);
                    i++;
                }
            }

            Console.WriteLine("  " + new string('-', 20));
        }
        private void ShowBook(Book book, int number = 0)
        {
            if (book != null)
            {
                var lineBookId = "  ";
                if (number > 0)
                {
                    lineBookId += $"{number}) ";
                }
                lineBookId += $"Book Id: {book.Id}";
                Console.WriteLine(lineBookId);
                Console.WriteLine($"  Name: {book.Name}");
                Console.WriteLine($"  Description: {book.Description}");
                Console.WriteLine($"  Pages: {book.Pages}");
                Console.WriteLine($"  Year: {book.Year}");
                if (book.BookAuthors.Count() > 0)
                {
                    Console.WriteLine("  Authors:");
                    var j = 1;
                    foreach (var author in book.BookAuthors)
                    {
                        Console.WriteLine($"    {j}. {author.FirstName} {author.LastName};");
                        j++;
                    }
                }
                if (book.BookSections.Count() > 0)
                {
                    Console.WriteLine("  Sections:");
                    var k = 1;
                    foreach (var section in book.BookSections)
                    {
                        Console.WriteLine($"    {k}. {section.Name};");
                        k++;
                    }
                }
            }
        }
        private void GetBooksLibrary()
        {
            Books = dataBaseLibrary.Books
                                     .Include(section => section.BookSections)
                                     .Include(author => author.BookAuthors)
                                     .ToList();
        }
        public void SearchBookLibraryByName()
        {
            enterNameBook = EnterNameBook();
            findBook = GetBookByName();
            ShowResultSearchBook();
        }
        public string EnterNameBook()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter name section, please:");
            var name = Console.ReadLine();
            if (name == "") EnterNameBook();
            return name;
        }
        private Book GetBookByName()
        {
            var book = dataBaseLibrary.Books
                                      .Where(book => book.Name == enterNameBook)
                                      .Include(book => book.BookSections)
                                      .Include(book => book.BookAuthors)
                                      .FirstOrDefault();
            return book;
        }
        public void ShowResultSearchBook()
        {
            Console.WriteLine($"Search book in library by name \"{enterNameBook}\":");
            if (findBook == null)
            {
                Console.WriteLine($"We not find book by name \"{enterNameBook}\" in our library.");
            }
            ShowBook(findBook);
        }

        public void AddBookLibraryByName()
        {
            enterNameBook = EnterNameBook();
            var bookExist = CheckExist(enterNameBook);
            if (!bookExist)
            {
                Set(FormingBook());
            }
            else
            {
                Console.WriteLine($"Book on name \"{enterNameBook}\" is exist in our library.");
                ShowBook(findBook);
            }
        }
        private bool CheckExist(string name)
        {
            var result = false;
            findBook = GetBookByName();
            if (findBook != null) result = true;
            return result;
        }
        private Book FormingBook()
        {
            var book = new Book()
            {
                Name = enterNameBook,
                Description = EnterDescription(),
                Year = EnterIntParam("book publication year"),
                Pages = EnterIntParam("count book`s pages")
            };
            var authors = AuthorManager.ChooseAuthors();
            if (authors != null) book.BookAuthors.AddRange(authors);
            var sections = SectionManager.ChooseSections();
            if(sections != null) book.BookSections.AddRange(sections);
            return book;
        }
        private string EnterDescription()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter description book, please:");
            var description = Console.ReadLine();
            return description;
        }
        private int EnterIntParam(string nameParam)
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter {nameParam}, please:");
            var value = 0;
            if(!int.TryParse(Console.ReadLine(), out value) && value > 0) EnterIntParam(nameParam);
            return value;
        }
        private void Set(Book book)
        {
            if (book != null)
            {
                dataBaseLibrary.Books.Add(book);
                dataBaseLibrary.SaveChanges();
            }
        }
        public void Edit()
        {
            SearchBookLibraryByName();
            if (findBook != null)
            {
                FormingBookForEdit();
                dataBaseLibrary.SaveChanges();
            }
        }
        private Book FormingBookForEdit()
        {
            if (ChooseEditOrNotParams("Name"))
            {
                findBook.Name = EnterNameBook();
            }
            if (ChooseEditOrNotParams("Description"))
            {
                findBook.Description = EnterDescription();
            }
            if (ChooseEditOrNotParams("Year"))
            {
                findBook.Year = EnterIntParam("book publication year");
            }
            if (ChooseEditOrNotParams("Pages"))
            {
                findBook.Pages = EnterIntParam("count book`s pages");
            }
            if (ChooseEditOrNotParams("Authors book"))
            {
                var authors = AuthorManager.ChooseAuthors();
                if (authors != null) findBook.BookAuthors.AddRange(authors);
            }
            if (ChooseEditOrNotParams("Sections book"))
            {
                var sections = SectionManager.ChooseSections();
                if (sections != null) findBook.BookSections.AddRange(sections);
            }
            return findBook;
        }
        private bool ChooseEditOrNotParams(string nameParams)
        {
            int number;
            var isEdit = false;
            var enterString = EnterNumberForEditOrNotParams(nameParams);
            if (int.TryParse(enterString, out number) && number == 1) isEdit = true;
            return isEdit;
        }
        private string EnterNumberForEditOrNotParams(string nameParams)
        {
            Console.WriteLine($"If you want edit \"{nameParams}\", enter number 1, please:");
            var enterString = Console.ReadLine();
            return enterString;
        }

        public List<Book> ChooseBooks()
        {
            ShowAllBooksLibrary();
            var choosesBooks = GetChooseBooks();
            return choosesBooks;
        }
        private List<Book> GetChooseBooks()
        {
            var choosePositionBooks = ChoosePositionBooks();
            var positions = MakeListPositions(choosePositionBooks);
            var books = MakeListChoosesBooks(positions);
            return books;
        }
        private string ChoosePositionBooks()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter the book number separated by a comma, please:");
            var booksPositionString = Console.ReadLine();
            return booksPositionString;
        }
        private List<int> MakeListPositions(string positionString)
        {
            var arrayPositions = positionString.Split(new char[] { ',' });
            var listPositions = new List<int>();
            var num = 0;
            foreach (var position in arrayPositions)
            {
                if (int.TryParse(position, out num) && num > 0 && num <= countBooks)
                {
                    listPositions.Add(num);
                }
            }
            return listPositions;
        }
        private List<Book> MakeListChoosesBooks(List<int> positions)
        {
            var listBooks = new List<Book>();
            foreach (var position in positions)
            {
                listBooks.Add(Books?.ElementAtOrDefault(position - 1));
            }
            return listBooks;
        }

    }
}
