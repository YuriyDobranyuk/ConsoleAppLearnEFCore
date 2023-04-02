using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Manager;

namespace ConsoleAppLearnEFCore.Menu
{
    public class LibraryMenu : BaseMenu, ILibraryMenu
    {
        public LibraryMenu(IServiceLibrary serviceLibrary, ILibraryMenu libraryMenu, ISectionMenu sectionMenu) : base(serviceLibrary, libraryMenu, sectionMenu)
        {
        }

        public void ShowLibraryMenu()
        {
            Console.Clear();
            DisplayTitle("Menu library:");
            Console.WriteLine($"For show menu sections library, enter number 1.");
            Console.WriteLine($"For show menu books library, enter number 2.");
            Console.WriteLine($"For show menu book`s authors, enter number 3.");
            Console.WriteLine($"For exit with library, enter number 0.");
            var selection = GetUserSelection();
            
            switch (selection)
            {
                case 0:
                    Environment.Exit(1);
                    break;
                case 1:
                    _sectionMenu.ShowMenuSectionLibrary();
                    break;
                /*case 2:
                    BookManager.ShowMenuBookLibrary();
                    break;
                case 3:
                    AuthorManager.ShowMenuAuthorLibrary();
                    break;*/
            }
            Console.WriteLine();
            Console.WriteLine("For continue enter key \"enter\"");
            Console.ReadLine();
            if (selection != 0) ShowLibraryMenu();
        }
    }
}
