using ConsoleAppLearnEFCore.Model;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAppLearnEFCore.Manager
{
    public class SectionManager
    {
        ApplicationDbContext dataBaseLibrary = new ApplicationDbContext();
       
        int countSections;
        int enterNumber;
        string enterNameSection;
        Section findLibrarySection;

        public List<Section>? Sections { get; set; }
        public BookManager BookManager { get; }

        public void ShowMenuSectionLibrary()
        {
            Console.WriteLine(new string('*', 20));
            Console.WriteLine("Menu library sections:");
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"For show library all sections, enter number 1.");
            Console.WriteLine($"For find library section, enter number 2.");
            Console.WriteLine($"For add library section, enter number 3.");
            Console.WriteLine($"For edit library section, enter number 4.");
            Console.WriteLine($"For return to menu library, enter number 0.");
            enterNumber = EnterNumber();
            switch (enterNumber)
            {
                case 1:
                    ShowAllSectionsLibrary();
                    break;
                case 2:
                    SearchSectionLibraryByName();
                    break;
                case 3:
                    AddSectionLibraryByName();
                    break;
                case 4:
                    Edit();
                    break;
            }
            Console.WriteLine();
            if(enterNumber != 0) ShowMenuSectionLibrary();
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
        private void ShowAllSectionsLibrary()
        {
            GetSectionsLibrary();
            countSections = Sections.Count();
            Console.WriteLine($"Sections library:");
            Console.WriteLine($"Count sections: {countSections}");
            if (countSections > 0)
            {
                var i = 1;
                foreach (var section in Sections)
                {
                    ShowSectionLibrary(section, i);
                    i++;
                }
            }
        }
        private void GetSectionsLibrary()
        {
            Sections = dataBaseLibrary.Sections
                                      .Include(section => section.BookSections)
                                      .ThenInclude(book => book.BookAuthors)
                                      .ToList();
        }
        private void SearchSectionLibraryByName()
        {
            enterNameSection = EnterNameSection();
            findLibrarySection = GetSectionLibraryByName();
            ShowResultSearchSectionLibrary();
        }
        private void ShowResultSearchSectionLibrary()
        {
            Console.WriteLine($"Search section library by name \"{enterNameSection}\":");
            if (findLibrarySection == null)
            {
                Console.WriteLine($"We not find section library by name \"{enterNameSection}\" in our library.");
            }
            ShowSectionLibrary(findLibrarySection);
        }
        private void ShowSectionLibrary(Section section, int number = 0)
        {
            if (section != null)
            {
                Console.WriteLine();
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
        private string EnterNameSection()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter name section, please:");
            var enterName = Console.ReadLine();
            if (enterName == "") EnterNameSection();
            return enterName;
        }
        private Section GetSectionLibraryByName()
        {
            var findSection = dataBaseLibrary.Sections
                                             .Where(section => section.Name == enterNameSection)
                                             .Include(section => section.BookSections)
                                             .ThenInclude(book => book.BookAuthors)
                                             .FirstOrDefault();
            return findSection;
        }
        private void AddSectionLibraryByName()
        {
            enterNameSection = EnterNameSection();
            var sectionExist = CheckExistSection(enterNameSection);
            if (!sectionExist)
            {
                Set(FormingSectionLibrary());
            }
            else
            {
                Console.WriteLine($"Section on name \"{enterNameSection}\" is exist in our library.");
                ShowSectionLibrary(findLibrarySection);
            }
        }
        private Section FormingSectionLibrary()
        {
            var section = new Section()
            {
                Name = enterNameSection,
                Description = EnterDescrioptionSection()
            };
            var books = BookManager.ChooseBooks();
            if (books != null) section.BookSections.AddRange(books);
            return section;
        }
        private bool CheckExistSection(string name)
        {
            var result = false;
            findLibrarySection = GetSectionLibraryByName();
            if (findLibrarySection != null) result = true;
            return result;
        }
        private string EnterDescrioptionSection()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter description section, please:");
            var descriptionSection = Console.ReadLine();
            return descriptionSection;
        }
        private void Set(Section section)
        {
            if (section != null)
            {
                dataBaseLibrary.Sections.Add(section);
                dataBaseLibrary.SaveChanges();
            }
        }
        
        public void Edit()
        {
            SearchSectionLibraryByName();
            if (findLibrarySection != null)
            {
                FormingSectionLibraryForEdit();
                dataBaseLibrary.SaveChanges();
            }
        }
        private Section FormingSectionLibraryForEdit()
        {
            if (ChooseEditOrNotParamsSection("Name"))
            {
                findLibrarySection.Name = EnterNameSection();
            }
            if (ChooseEditOrNotParamsSection("Description"))
            {
                findLibrarySection.Description = EnterDescrioptionSection();
            }
            if (ChooseEditOrNotParamsSection("Books section"))
            {
                var books = BookManager.ChooseBooks();
                if (books != null) findLibrarySection.BookSections.AddRange(books);
            }
            return findLibrarySection;
        }

        private bool ChooseEditOrNotParamsSection(string nameParams)
        {
            int number;
            var isEdit = false;
            var enterString = EnterNumberForEditOrNotParamsSection(nameParams);
            if (int.TryParse(enterString, out number) && number == 1) isEdit = true;
            return isEdit;
        }
        private string EnterNumberForEditOrNotParamsSection(string nameParams)
        {
            Console.WriteLine($"If you want edit \"{nameParams}\", enter number 1, please:");
            var enterString = Console.ReadLine();
            return enterString;
        }
        public List<Section> ChooseSections()
        {
            ShowAllSectionsLibrary();
            var choosesSections = GetChooseSections();
            return choosesSections;
        }
        private List<Section> GetChooseSections()
        {
            var choosePositionSections = ChoosePositionSections();
            var positions = MakeListPositions(choosePositionSections);
            var sections = MakeListChoosesSections(positions);
            return sections;
        }
        private string ChoosePositionSections()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter the section number separated by a comma, please:");
            var sectionsPositionString = Console.ReadLine();
            return sectionsPositionString;
        }
        private List<int> MakeListPositions(string positionString)
        {
            var arrayPositions = positionString.Split(new char[] { ',' });
            var listPositions = new List<int>();
            var num = 0;
            foreach (var position in arrayPositions)
            {
                if (int.TryParse(position, out num) && num > 0 && num <= countSections)
                {
                    listPositions.Add(num);
                }
            }
            return listPositions;
        }
        private List<Section> MakeListChoosesSections(List<int> positions)
        {
            var listSections = new List<Section>();
            foreach (var position in positions)
            {
                listSections.Add(Sections?.ElementAtOrDefault(position - 1));
            }
            return listSections;
        }

    }
}
