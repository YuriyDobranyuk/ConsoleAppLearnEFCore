using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Model;
using Microsoft.AspNetCore.Components;

namespace ConsoleAppLearnEFCore.Menu
{
    public class AuthorMenu : BaseMenu
    {
        private string _enterFirstName;
        private string _enterLastName;
        private IList<Author>? _authors;
        private int _countAuthors;
        private Author? _findedAuthor;

        public EventCallback BackToLibraryMenu { get; set; }
        public AuthorMenu(IServiceLibrary serviceLibrary) : base(serviceLibrary) 
        {
            var _sections = _serviceLibrary.GetAllItems<Section>(x => x.BookSections).Result;
        }


        public async Task ShowMenuAuthor()
        {
            Console.Clear();
            DisplayTitle("Menu library authors:");
            Console.WriteLine($"For show all authors, enter number 1.");
            Console.WriteLine($"For find author, enter number 2.");
            Console.WriteLine($"For add author to library, enter number 3.");
            Console.WriteLine($"For edit author, enter number 4.");
            Console.WriteLine($"For delete author, enter number 5.");
            Console.WriteLine($"For return to menu library, enter number 0.");
            var selection = GetUserSelection();
            switch (selection)
            {
                case 0:
                    await BackToLibraryMenu.InvokeAsync();
                    break;
                case 1:
                    ShowAllAuthorsLibrary();
                    break;
                case 2:
                    SearchAuthorLibraryByNames();
                    break;
                case 3:
                    AddAuthorToLibrary();
                    break;
                case 4:
                    EditAuthor();
                    break;
                case 5:
                    DeleteAuthorWithLibrary();
                    break;
            }
            EnterKeyForContinueWork();
            if (selection != 0) await ShowMenuAuthor();
        }

        public void ShowAllAuthorsLibrary()
        {
            _authors = _serviceLibrary.GetAllItems<Author>(x => x.BookAuthors).Result;
            _countAuthors = _authors.Count;

            Console.WriteLine($"Authors library:");
            Console.WriteLine($"Count authors: {_countAuthors}");
            if (_countAuthors > 0)
            {
                var i = 1;
                foreach (var author in _authors)
                {
                    ShowAuthorLibrary(author, i);
                    i++;
                }
            }
        }

        private void ShowAuthorLibrary(Author? author, int number = 0)
        {
            if (author != null)
            {
                var lineAuthor = "  ";
                if (number > 0)
                {
                    lineAuthor += $"{number}) ";
                }
                lineAuthor += $"{author.FirstName} {author.LastName} (Id: {author.Id})";
                Console.WriteLine(lineAuthor);

                if (author.BookAuthors.Count() > 0)
                {
                    Console.WriteLine("  Books:");
                    var i = 1;
                    foreach (var book in author.BookAuthors)
                    {
                        Console.WriteLine($"    {i}. Book Id: {book.Id}");
                        Console.WriteLine($"    Book name: {book.Name}");
                        Console.WriteLine($"    Book description: {book.Description}");
                        Console.WriteLine($"    Book pages: {book.Pages}");
                        Console.WriteLine($"    Book year: {book.Year}");
                        Console.WriteLine($"    Count book Authors: {book.BookAuthors.Count()}");
                        if (book.BookAuthors.Count() > 0)
                        {
                            Console.WriteLine("    Book authors:");
                            var j = 1;
                            foreach (var authorBook in book.BookAuthors)
                            {
                                Console.WriteLine($"      {j}. {authorBook.FirstName} {authorBook.LastName} (Id: {authorBook.Id})");
                                j++;
                            }
                        }
                        if (book.BookSections.Count() > 0)
                        {
                            Console.WriteLine("    Sections:");
                            var k = 1;
                            foreach (var section in book.BookSections)
                            {
                                Console.WriteLine($"     {k}. {section.Name};");
                                k++;
                            }
                        }
                        i++;
                    }
                }
                Console.WriteLine(new string('-', 20));
            }
        }

        private void SearchAuthorLibraryByNames()
        {
            _enterLastName = EnterPropertyValue("LastName", "author", true);
            _enterFirstName = EnterPropertyValue("FirstName", "author", true);
            _findedAuthor = GetAuthorByName();
            ShowResultSearchAuthor();
        }

        private Author GetAuthorByName()
        {
            return _serviceLibrary.Get<Author>(x => x.LastName == _enterLastName 
                                                    && x.FirstName == _enterFirstName,
                                                    x => x.BookAuthors);
        }

        public void ShowResultSearchAuthor()
        {
            Console.WriteLine($"Search author in library by lastname \"{_enterLastName}\" and firstname \"{_enterFirstName}\":");
            if (_findedAuthor == null)
            {
                Console.WriteLine($"We not find author by lastname \"{_enterLastName}\" and firstname \"{_enterFirstName}\" in our library.");
            }
            ShowAuthorLibrary(_findedAuthor);
        }

        public Author AddAuthorToLibrary()
        {
            Author addedAuthor;
            _enterLastName = EnterPropertyValue("LastName", "author", true);
            _enterFirstName = EnterPropertyValue("FirstName", "author", true);
            _findedAuthor = GetAuthorByName();
            var checkExitBook = _serviceLibrary.CheckExist<Author>(_findedAuthor);

            if (!checkExitBook)
            {
                _serviceLibrary.Add<Author>(FormingAuthor());
                addedAuthor = GetAuthorByName();
                Console.WriteLine($"You are add new author with lastname \"{_enterLastName}\" and firstname \"{_enterFirstName}\" in our library.");
            }
            else
            {
                Console.WriteLine($"Author by lastname \"{_enterLastName}\" and firstname \"{_enterFirstName}\" is exist in our library.");
                addedAuthor = _findedAuthor;
            }
            ShowAuthorLibrary(addedAuthor);
            return addedAuthor;
        }

        private Author FormingAuthor()
        {
            return new Author()
            {
                FirstName = _enterFirstName,
                LastName = _enterLastName
            };
        }

        public void EditAuthor()
        {
            SearchAuthorLibraryByNames();
            if (_findedAuthor != null)
            {
                FormingAuthorForEdit();
                _serviceLibrary.Update<Author>(_findedAuthor);
                ShowAuthorLibrary(_findedAuthor);
            }
        }

        private Author FormingAuthorForEdit()
        {
            if (ChooseEditOrNotParams("LastName"))
            {
                _findedAuthor.FirstName = EnterPropertyValue("LastName", "author", true);
            }
            if (ChooseEditOrNotParams("FirstName"))
            {
                _findedAuthor.FirstName = EnterPropertyValue("FirstName", "author", true);
            }
            return _findedAuthor;
        }

        private void DeleteAuthorWithLibrary()
        {
            SearchAuthorLibraryByNames();
            var confirmDelete = ConfirmDeleteItem("author");
            if (confirmDelete) _serviceLibrary.Delete<Author>(_findedAuthor);
        }

        public List<Author> SelectAuthors()
        {
            ShowAllAuthorsLibrary();
            var authors = GetSelectAuthors();
            return authors;
        }

        private List<Author> GetSelectAuthors()
        {
            var choosePositionBooks = ChoosePosition("author");
            var positions = MakeListPositions(choosePositionBooks, _countAuthors);
            var authors = MakeListChooses<Author>(positions, _authors);
            return authors;
        }

    }
}
