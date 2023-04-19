using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Manager;
using ConsoleAppLearnEFCore.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Section = ConsoleAppLearnEFCore.Model.Section;


namespace ConsoleAppLearnEFCore.Menu
{
    public class SectionMenu : BaseMenu, ISectionMenu
    {
        private string _enterNameSection;
        private IList<Section> _sectionsLibrary;
        private int _countSections;
        private Section? _findedSectionLibrary;

        public EventCallback BackToLibraryMenu { get; set; }
        public BookMenu BookMenu { get; set; }

        //public EventCallback<List<Book>> BackToChooseBooks { get; set; }

        public SectionMenu(IServiceLibrary serviceLibrary) : base(serviceLibrary)
        {
            BookMenu = new BookMenu(serviceLibrary);
        }
        
        public async Task ShowMenuSectionLibrary()
        {
            Console.Clear();
            DisplayTitle("Menu library sections:");
            Console.WriteLine($"For show library all sections, enter number 1.");
            Console.WriteLine($"For find library section, enter number 2.");
            Console.WriteLine($"For add library section, enter number 3.");
            Console.WriteLine($"For edit library section, enter number 4.");
            Console.WriteLine($"For delete library section, enter number 5.");
            Console.WriteLine($"For return to menu library, enter number 0.");
            var selection = GetUserSelection();
            switch (selection)
            {
                case 0:
                    await BackToLibraryMenu.InvokeAsync();
                    break;
                case 1:
                    ShowAllSectionsLibrary();
                    break;
                case 2:
                    SearchSectionLibraryByName();
                    break;
                case 3:
                    AddSectionToLibrary();
                    break;
                case 4:
                    EditSectionLibrary();
                    break;
                case 5:
                    DeleteSectionWithLibrary();
                    break;
            }
            EnterKeyForContinueWork();
            if (selection != 0) ShowMenuSectionLibrary();
        }

        private void ShowAllSectionsLibrary()
        {
            var books = _serviceLibrary.GetAllItems<Book>(x => x.BookSections, x => x.BookAuthors).Result;
            _sectionsLibrary = _serviceLibrary.GetAllItems<Section>(x => x.BookSections).Result;
            _countSections = _sectionsLibrary.Count;
                        
            Console.WriteLine($"Sections library:");
            Console.WriteLine($"Count sections: {_countSections}");
            if (_countSections > 0)
            {
                var i = 1;
                foreach (var section in _sectionsLibrary)
                {
                    ShowSectionLibrary(section, i);
                    i++;
                }
            }
        }

        private void ShowSectionLibrary(Section section, int number = 0)
        {
            if (section != null)
            {
                var lineSectionId = "";
                if (number > 0)
                {
                    lineSectionId += $"{number}) ";
                }
                lineSectionId += $"Section Id: {section.Id}";
                Console.WriteLine(lineSectionId);
                Console.WriteLine($"Name: {section.Name}");
                Console.WriteLine($"Description: {section.Description}");
                Console.WriteLine($"Count books: {section.BookSections.Count()}");
                if (section.BookSections.Count() > 0)
                {
                    var i = 1;
                    foreach (var book in section.BookSections)
                    {
                        Console.WriteLine($"  {i}) Book Id: {book.Id}");
                        Console.WriteLine($"  Book name: {book.Name}");
                        Console.WriteLine($"  Book description: {book.Description}");
                        Console.WriteLine($"  Count book Authors: {book.BookAuthors.Count()}");
                        if (book.BookAuthors.Count() > 0)
                        {
                            Console.WriteLine("  Book authors:");
                            var j = 1;
                            foreach (var author in book.BookAuthors)
                            {
                                Console.WriteLine($"    {j}. {author.LastName} {author.FirstName} (Author Id: {author.Id})");
                                j++;
                            }
                        }
                        Console.WriteLine($"  Book pages: {book.Pages}");
                        Console.WriteLine($"  Book year: {book.Year}");
                        i++;
                    }
                }
                Console.WriteLine(new string('-', 20));
            }
        }

        private void SearchSectionLibraryByName()
        {
            _enterNameSection = EnterPropertyValue("name", "section", true);
            _findedSectionLibrary = GetSectionByName();
            ShowResultSearchSectionLibrary();
        }

        private Section GetSectionByName()
        {
            return _serviceLibrary.Get<Section>(x => x.Name == _enterNameSection, x => x.BookSections);
        }

        public void ShowResultSearchSectionLibrary()
        {
            Console.WriteLine($"Search section in library by name \"{_enterNameSection}\":");
            if (_findedSectionLibrary == null)
            {
                Console.WriteLine($"We not find section by name \"{_enterNameSection}\" in our library.");
            }
            ShowSectionLibrary(_findedSectionLibrary);
        }

        private void AddSectionToLibrary()
        {
            _enterNameSection = EnterPropertyValue("name", "section", true);
            _findedSectionLibrary = GetSectionByName();
            var checkExitSection = _serviceLibrary.CheckExist<Section>(_findedSectionLibrary);

            if (!checkExitSection)
            {
                _serviceLibrary.Add<Section>(FormingSection());
                Console.WriteLine($"You are add new section with name \"{_enterNameSection}\" in our library.");
            }
            else
            {
                Console.WriteLine($"Section on name \"{_enterNameSection}\" is exist in our library.");
                ShowSectionLibrary(_findedSectionLibrary);
            }
        }

        private Section FormingSection()
        {
            var section = new Section()
            {
                Name = _enterNameSection,
                Description = EnterPropertyValue("description", "section", true),
            };
            List<Book> booksForSectionLibrary = GetBooksForAddToSectionLibrary();
            if (booksForSectionLibrary != null) section.BookSections.AddRange(booksForSectionLibrary);
            return section;
        }

        private List<Book> GetBooksForAddToSectionLibrary()
        {
            var allBook = new List<Book>();
            for (int i = 1; i == 1; i = GetUserSelection())
            {
                allBook.AddRange(FormingBooksToSectionLibrary());
                Console.WriteLine($"For create or select books for library`s section, enter number 1.");
                Console.WriteLine($"For end create and select books, enter anywhere key.");
                Console.WriteLine();
            }
            return allBook;
        }
        private List<Book> FormingBooksToSectionLibrary()
        {
            var books = new List<Book>();
            Console.WriteLine($"For create new books and add to library`s section, enter number 1.");
            Console.WriteLine($"For select books for add to library`s section, enter number 2.");
            Console.WriteLine($"For not add books to library`s section, enter anywhere key.");
            var selection = GetUserSelection();
            switch (selection)
            {
                case 1:
                    books = AddNewBooksToLibrary();
                    break;
                case 2:
                    books = SelectBooksToSectionLibrary();
                    break;
            }
            return books;
        }
        
        private List<Book> AddNewBooksToLibrary()
        {
            var books = new List<Book>();
            for (int i = 1; i == 1; i = GetUserSelection())
            {
                books.Add(BookMenu.AddBookToLibrary());
            }
            return books;
        }
        
        private List<Book> SelectBooksToSectionLibrary()
        {
            return BookMenu.SelectBooks();
        }

        public void EditSectionLibrary()
        {
            SearchSectionLibraryByName();
            if (_findedSectionLibrary != null)
            {
                FormingSectionLibraryForEdit();
                _serviceLibrary.Update<Section>(_findedSectionLibrary); 
            }
        }

        private Section FormingSectionLibraryForEdit()
        {
            if (ChooseEditOrNotParams("Name"))
            {
                _findedSectionLibrary.Name = EnterPropertyValue("name", "book", true);
            }
            if (ChooseEditOrNotParams("Description"))
            {
                _findedSectionLibrary.Description = EnterPropertyValue("description", "book", true);
            }
            if (ChooseEditOrNotParams("Books section"))
            {
                List<Book> booksForSectionLibrary = GetBooksForAddToSectionLibrary();
                if (booksForSectionLibrary != null) _findedSectionLibrary.BookSections.AddRange(booksForSectionLibrary);
            }
            return _findedSectionLibrary;
        }

        private void DeleteSectionWithLibrary()
        {
            SearchSectionLibraryByName();
            var confirmDelete = ConfirmDeleteItem("section");
            if (confirmDelete) _serviceLibrary.Delete(_findedSectionLibrary);
        }

    }
}
