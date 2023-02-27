namespace ConsoleAppLearnEFCore.Manager
{
    public class LibraryManager
    {
        
        public SectionManager SectionManager { get; } = new SectionManager();
        public BookManager BookManager { get; } =  new BookManager();
        public AuthorManager AuthorManager { get; } = new AuthorManager();  

        int enterNumber;

        public void ShowMenuLibrary()
        {
            Console.WriteLine(new string('*', 20));
            Console.WriteLine("Menu library:");
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"For show menu sections library, enter number 1.");
            Console.WriteLine($"For show menu books library, enter number 2.");
            Console.WriteLine($"For show menu book`s authors, enter number 3.");
            Console.WriteLine($"For exit with library, enter number 0.");

            enterNumber = EnterNumber();
            switch (enterNumber)
            {
                case 0:
                    Environment.Exit(1);
                    break;
                case 1:
                    SectionManager.ShowMenuSectionLibrary();
                    break;
                case 2:
                    BookManager.ShowMenuBookLibrary();
                    break;
                case 3:
                    AuthorManager.ShowMenuAuthorLibrary();
                    break;
            }
            Console.WriteLine();
            if(enterNumber != 0) ShowMenuLibrary();
        }
        public int EnterNumber()
        {
            Console.WriteLine(new string('*', 10));
            Console.WriteLine($"Enter number, please:");
            var userEnterString = Console.ReadLine();
            var resultConvert = int.TryParse(userEnterString, out enterNumber);
            if (!resultConvert) EnterNumber();
            return enterNumber;
        }
    }
}
