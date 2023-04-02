using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Model;
using Section = ConsoleAppLearnEFCore.Model.Section;


namespace ConsoleAppLearnEFCore.Menu
{
    public class SectionMenu : BaseMenu, ISectionMenu
    {
        public SectionMenu(IServiceLibrary serviceLibrary, ILibraryMenu libraryMenu, ISectionMenu sectionMenu) : base(serviceLibrary, libraryMenu, sectionMenu)
        {
        }

        public void ShowMenuSectionLibrary()
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
                    _libraryMenu.ShowLibraryMenu();
                    break;
                case 1:
                    ShowAllSectionsLibrary();
                    break;
                case 2:
                    ShowSearchSectionLibrary();
                    break;
                case 3:
                    ShowOrAddSectionLibrary();
                    break;
                case 4:
                    //_sectionService.EditSection();
                    break;
                case 5:
                    //_sectionService.EditSection();
                    break;
            }
            Console.WriteLine("For continue enter key \"enter\"");
            Console.ReadLine();
            if (selection != 0) ShowMenuSectionLibrary();
        }

        private void ShowAllSectionsLibrary()
        {
            var sections = _serviceLibrary.GetAllItems<Section>(x => x.BookSections).Result;
            var books = _serviceLibrary.GetAllItems<Book>(x => x.BookSections, x => x.BookAuthors).Result;
            
            Console.WriteLine($"Sections library:");
            Console.WriteLine($"Count sections: {sections.Count}");
            if (sections.Count > 0)
            {
                var i = 1;
                foreach (var section in sections)
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

        private string EnterSectionProperty(string property, bool required)
        {
            Console.WriteLine(new string('_', 10));
            Console.WriteLine($"Enter {property} section, please:");
            var enterName = Console.ReadLine();
            if (string.IsNullOrEmpty(enterName) && required) EnterSectionProperty(property, required);
            return enterName;
        }

        private void ShowOrAddSectionLibrary()
        {
            var section = new Section()
            {
                Name = EnterSectionProperty("name", true),
                Description = EnterSectionProperty("description", true),
                //BookSections = BookService.ChooseBooks()
            };

            if (!_serviceLibrary.CheckExistByName<Section>(x => x.Name == section.Name, x => x.BookSections))
            {
                _serviceLibrary.Add<Section>(section);
            }
            else
            {
                Console.WriteLine($"Section on name \"{section.Name}\" is exist in our library.");
            }
        }

        private void ShowSearchSectionLibrary()
        {
            var sectionName = EnterSectionProperty("name", true);
            var section = _serviceLibrary.Get<Section>(x => x.Name == sectionName, x => x.BookSections);
            Console.WriteLine($"Search section library by name \"{sectionName}\":");
            if (section == null)
            {
                Console.WriteLine($"We not find section library by name \"{sectionName}\" in our library.");
            }
            ShowSectionLibrary(section);
        }

        

        
    }
}
