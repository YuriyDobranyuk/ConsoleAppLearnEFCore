using ConsoleAppLearnEFCore.Model;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAppLearnEFCore.Manager
{
    public class AuthorManager
    {
        ApplicationDbContext dataBaseLibrary = new ApplicationDbContext();
        
        int enterNumber;
        string enterLastName;
        string enterFirstName;
        Author findAuthor;
        int countAuthors;

        public List<Author>? Authors { get; set; }
        public BookManager BookManager { get; }
        public void ShowMenuAuthorLibrary()
        {
            Console.WriteLine(new string('*', 20));
            Console.WriteLine("Menu authors books:");
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"For show authors, enter number 1.");
            Console.WriteLine($"For find author book, enter number 2.");
            Console.WriteLine($"For add author book, enter number 3.");
            Console.WriteLine($"For edit author book, enter number 4.");
            Console.WriteLine($"For return to menu library, enter number 0.");
            enterNumber = EnterNumber();
            switch (enterNumber)
            {
                case 1:
                    ShowAllAuthors();
                    break;
                case 2:
                    SearchAuthorByName();
                    break;
                case 3:
                    AddAuthorByName();
                    break;
                case 4:
                    Edit();
                    break;
            }
            Console.WriteLine();
            if (enterNumber != 0) ShowMenuAuthorLibrary();
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
        public void ShowAllAuthors()
        {
            GetAuthorsLibrary();

            countAuthors = Authors.Count();
            Console.WriteLine($"Authors library:");
            Console.WriteLine($"Count authors: {countAuthors}");
            if (countAuthors > 0)
            {
                var i = 1;
                foreach (var author in Authors)
                {
                    ShowAuthor(author, i);
                    i++;
                }
            }
            Console.WriteLine("  " + new string('-', 20));
        }
        private void ShowAuthor(Author author, int number = 0)
        {
            if (author != null)
            {
                var lineAuthor = "  ";
                if (number > 0)
                {
                    lineAuthor += $"{number}) ";
                }
                lineAuthor += $"{author.LastName} {author.FirstName} (Id: {author.Id})";
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
                        Console.WriteLine($"    Count book Authors: {book.BookAuthors.Count()}");
                        if (book.BookAuthors.Count() > 0)
                        {
                            Console.WriteLine("    Book authors:");
                            var j = 1;
                            foreach (var authorBook in book.BookAuthors)
                            {
                                Console.WriteLine($"      {j}. {authorBook.LastName} {authorBook.FirstName} (Id: {authorBook.Id})");
                                j++;
                            }
                        }
                        Console.WriteLine($"    Book pages: {book.Pages}");
                        Console.WriteLine($"    Book year: {book.Year}");
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
            }
        }
        private void GetAuthorsLibrary()
        {
            Authors = dataBaseLibrary.Authors
                                     .Include(author => author.BookAuthors)
                                     .ThenInclude(book => book.BookSections)
                                     .ToList();
        }
        public void SearchAuthorByName()
        {
            enterLastName = EnterName("author`s last name");
            enterFirstName = EnterName("author`s first name");
            findAuthor = GetAuthorByName();
            ShowResultSearchAuthor();
        }
        private string EnterName(string nameParametr)
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter {nameParametr}, please:");
            var name = Console.ReadLine();
            if (name == "") EnterName(nameParametr);
            return name;
        }
        private Author GetAuthorByName()
        {
            var author = dataBaseLibrary.Authors
                                        .Where(author => (author.LastName == enterLastName 
                                                     && author.FirstName == enterFirstName))
                                        .Include(author => author.BookAuthors)
                                        .ThenInclude(book => book.BookSections)
                                        .FirstOrDefault();
            return author;
        }
        private void ShowResultSearchAuthor()
        {
            Console.WriteLine($"Search author in library by name \"{enterLastName} {enterFirstName}\":");
            if (findAuthor == null)
            {
                Console.WriteLine($"We not find author by name \"{enterLastName} {enterFirstName}\" in our library.");
            }
            ShowAuthor(findAuthor);
        }
        public void AddAuthorByName()
        {
            enterLastName = EnterName("author`s last name");
            enterFirstName = EnterName("author`s first name");
            var authorExist = CheckExistAuthor(enterLastName, enterFirstName);
            if (!authorExist)
            {
                var author = FormingAuthor();
                Set(author);
            }
            else
            {
                Console.WriteLine($"Author on name \"{enterLastName} {enterFirstName}\" is exist in our library.");
                ShowAuthor(findAuthor);
            }
        }
        private bool CheckExistAuthor(string lastName, string firstName)
        {
            var result = false;
            findAuthor = GetAuthorByName();
            if (findAuthor != null) result = true;
            return result;
        }
        private Author FormingAuthor()
        {
            var author = new Author()
            {
                FirstName = enterFirstName,
                LastName = enterLastName
            };
            var books = BookManager.ChooseBooks();
            if (books != null) author.BookAuthors.AddRange(books);
            return author;
        }
        private void Set(Author author)
        {
            if (author != null)
            {
                dataBaseLibrary.Authors.Add(author);
                dataBaseLibrary.SaveChanges();
            }
        }
        public void Edit()
        {
            SearchAuthorByName();
            if (findAuthor != null)
            {
                FormingAuthorForEdit();
                dataBaseLibrary.SaveChanges();
            }
        }
        private Author FormingAuthorForEdit()
        {
            if (ChooseEditOrNotParams("FirstName"))
            {
                findAuthor.FirstName = EnterName("author`s first name");
            }
            if (ChooseEditOrNotParams("LastName"))
            {
                findAuthor.LastName = EnterName("author`s last name");
            }
            if (ChooseEditOrNotParams("Author`s books"))
            {
                var books = BookManager.ChooseBooks();
                if (books != null) findAuthor.BookAuthors.AddRange(books);
            }
            return findAuthor;
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
        public List<Author> ChooseAuthors()
        {
            ShowAllAuthors();
            var choosesAuthors = GetChooseAuthors();
            return choosesAuthors;
        }
        private List<Author> GetChooseAuthors()
        {
            var choosePositionAuthors = ChoosePositionAuthors();
            var positions = MakeListPositions(choosePositionAuthors);
            var authors = MakeListChoosesAuthors(positions);
            return authors;
        }
        private string ChoosePositionAuthors()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter the author number separated by a comma, please:");
            var authorsPositionString = Console.ReadLine();
            return authorsPositionString;
        }
        private List<int> MakeListPositions(string positionString)
        {
            var arrayPositions = positionString.Split(new char[] { ',' });
            var listPositions = new List<int>();
            var num = 0;
            foreach (var position in arrayPositions)
            {
                if (int.TryParse(position, out num) && num > 0 && num <= countAuthors)
                {
                    listPositions.Add(num);
                }
            }
            return listPositions;
        }
        private List<Author> MakeListChoosesAuthors(List<int> positions)
        {
            var listAuthors = new List<Author>();
            foreach (var position in positions)
            {
                listAuthors.Add(Authors?.ElementAtOrDefault(position - 1));
            }
            return listAuthors;
        }
    }
}
