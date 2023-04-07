using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Manager;
using ConsoleAppLearnEFCore.Model;
using Microsoft.AspNetCore.Components;
using System.Security.AccessControl;
using static System.Collections.Specialized.BitVector32;
using static System.Reflection.Metadata.BlobBuilder;

namespace ConsoleAppLearnEFCore.Menu
{
    public class BookMenu : BaseMenu
    {
        private string _enterNameBook;
        private IList<Book>? _books;
        private int _countBooks;
        private Book? _findedBook;
       
        public EventCallback BackToLibraryMenu { get; set; }

        public AuthorMenu AuthorMenu { get; set; }

        public BookMenu(IServiceLibrary serviceLibrary) : base(serviceLibrary)
        {
            /*SectionMenu = new SectionMenu(serviceLibrary)
            {
                //BackToChooseBooks = EventCallback.Factory.Create(this, (List<Book> books) => ChooseBooks(books))
            };*/
            AuthorMenu = new AuthorMenu(serviceLibrary);
        }
        
        public async Task ShowMenuBookLibrary()
        {
            Console.Clear();
            DisplayTitle("Menu library books:");
            Console.WriteLine($"For show all books, enter number 1.");
            Console.WriteLine($"For find book, enter number 2.");
            Console.WriteLine($"For add book to library, enter number 3.");
            Console.WriteLine($"For edit book, enter number 4.");
            Console.WriteLine($"For delete book, enter number 5.");
            Console.WriteLine($"For return to menu library, enter number 0.");
            var selection = GetUserSelection();
            switch (selection)
            {
                case 0:
                    await BackToLibraryMenu.InvokeAsync();
                    break;
                case 1:
                    ShowAllBooksLibrary();
                    break;
                case 2:
                    SearchBookLibraryByName();
                    break;
                case 3:
                    AddBookToLibrary();
                    break;
                case 4:
                    EditBook();
                    break;
                case 5:
                    DeleteBookWithLibrary();
                    break;
            }
            EnterKeyForContinueWork();
            if (selection != 0) ShowMenuBookLibrary();
        }

        public void ShowAllBooksLibrary()
        {
            _books = _serviceLibrary.GetAllItems<Book>(x => x.BookSections, x => x.BookAuthors).Result;
            _countBooks = _books.Count;

            Console.WriteLine($"Books library:");
            Console.WriteLine($"Count books: {_countBooks}");
            if (_countBooks > 0)
            {
                var i = 1;
                foreach (var book in _books)
                {
                    ShowBookLibrary(book, i);
                    i++;
                }
            }
        }

        private void ShowBookLibrary(Book? book, int number = 0)
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
                Console.WriteLine(new string('-', 20));
            }
        }

        private void SearchBookLibraryByName()
        {
            _enterNameBook = EnterPropertyValue("name", "book", true);
            _findedBook = GetBookByName();
            ShowResultSearchBook();
        }

        private Book GetBookByName()
        {
            return _serviceLibrary.Get<Book>(x => x.Name == _enterNameBook,
                                                    x => x.BookSections,
                                                    x => x.BookAuthors);
        }
        
        public void ShowResultSearchBook()
        {
            Console.WriteLine($"Search book in library by name \"{_enterNameBook}\":");
            if (_findedBook == null)
            {
                Console.WriteLine($"We not find book by name \"{_enterNameBook}\" in our library.");
            }
            ShowBookLibrary(_findedBook);
        }

        public Book AddBookToLibrary()
        {
            Book addedBook;
            _enterNameBook = EnterPropertyValue("name", "book", true);
            _findedBook = GetBookByName();
            var checkExitBook = _serviceLibrary.CheckExist<Book>(_findedBook);

            if (!checkExitBook)
            {
                _serviceLibrary.Add<Book>(FormingBook());
                addedBook = GetBookByName();
                Console.WriteLine($"You are add new book with name \"{_enterNameBook}\" in our library.");
            }
            else
            {
                Console.WriteLine($"Section on name \"{_enterNameBook}\" is exist in our library.");
                addedBook = _findedBook;
            }
            ShowBookLibrary(addedBook);
            return addedBook;
        }

        private Book FormingBook()
        {
            var book = new Book()
            {
                Name = EnterPropertyValue("name", "book", true),
                Description = EnterPropertyValue("description", "book", true),
                Year = EnterIntPropertyValue("book publication year"),
                Pages = EnterIntPropertyValue("count book`s pages")
            };
            var authorsForBook = GetAuthorsForAddToBook();
            if (authorsForBook != null) book.BookAuthors.AddRange(authorsForBook);
            return book;
        }

        private List<Book> GetAuthorsForAddToBook()
        {
            var allAuthors = new List<Author>();
            Console.WriteLine($"For create or select authors for book, enter number 1.");
            Console.WriteLine($"For end create and select authors, enter anywhere key.");
            for (int i = 1; i == 1; i = GetUserSelection())
            {
                allAuthors.AddRange(FormingAuthorsToBook());
            }
            return allAuthors;
        }
        
        private List<Author> FormingAuthorsToBook()
        {
            var authors = new List<Author>();
            Console.WriteLine($"For create new authors and add to book, enter number 1.");
            Console.WriteLine($"For select authors for add to book, enter number 2.");
            Console.WriteLine($"For not add authors to book, enter anywhere key.");
            var selection = GetUserSelection();
            switch (selection)
            {
                case 1:
                    authors = AddNewAuthorsToLibrary();
                    break;
                case 2:
                    authors = SelectAuthorsToSectionLibrary();
                    break;
            }
            return authors;
        }

        private List<Author> AddNewAuthorsToLibrary()
        {
            var authors = new List<Author>();
            for (int i = 1; i == 1; i = GetUserSelection())
            {
                authors.Add(AuthorMenu.AddAuthorToLibrary());
            }
            return authors;
        }

        private List<Author> SelectAuthorsToBook()
        {
            return AuthorMenu.SelectAuthors();
        }

        public void EditBook()
        {
            SearchBookLibraryByName();
            if (_findedBook != null)
            {
                FormingBookForEdit();
                _serviceLibrary.Update<Book>(_findedBook);
            }
        }

        private Book FormingBookForEdit()
        {
            if (ChooseEditOrNotParams("Name"))
            {
                _findedBook.Name = EnterPropertyValue("name", "book", true);
            }
            if (ChooseEditOrNotParams("Description"))
            {
                _findedBook.Description = EnterPropertyValue("description", "book", true);
            }
            if (ChooseEditOrNotParams("Year"))
            {
                _findedBook.Year = EnterIntPropertyValue("year");
            }
            if (ChooseEditOrNotParams("Pages"))
            {
                _findedBook.Pages = EnterIntPropertyValue("count pages");
            }
            
            if (ChooseEditOrNotParams("Authors section"))
            {
                var authorsForBook = GetAuthorsForAddToBook();
                if (authorsForBook != null) _findedBook.BookAuthors.AddRange(authorsForBook);
            }
            return _findedBook;
        }

        private void DeleteBookWithLibrary()
        {
            SearchBookLibraryByName();
            var confirmDelete = ConfirmDeleteItem("book");
            if (confirmDelete) _serviceLibrary.Delete<Book>(_findedBook);
        }
       
        public List<Book> SelectBooks()
        {
            ShowAllBooksLibrary();
            var books = GetSelectBooks();
            return books;
        }

        private List<Book> GetSelectBooks()
        {
            var choosePositionBooks = ChoosePosition("book");
            var positions = MakeListPositions(choosePositionBooks, _countBooks);
            var books = MakeListChooses<Book>(positions, _books);
            return books;
        }
        

    }
}
