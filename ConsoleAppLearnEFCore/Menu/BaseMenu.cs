using ConsoleAppLearnEFCore.Interface;

namespace ConsoleAppLearnEFCore.Menu
{
    public class BaseMenu
    {
        public IServiceLibrary _serviceLibrary;
        public ILibraryMenu _libraryMenu;
        public ISectionMenu _sectionMenu;
        private IServiceLibrary serviceLibrary;

        public BaseMenu(ISectionMenu sectionMenu) 
        {
            _sectionMenu = sectionMenu;
        }
        public BaseMenu(IServiceLibrary serviceLibrary, ILibraryMenu libraryMenu, ISectionMenu sectionMenu) 
        {
            _serviceLibrary = serviceLibrary;
            _libraryMenu = libraryMenu;
            _sectionMenu = sectionMenu;
        }

        public void DisplayTitle(string title)
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"{title}");
            Console.WriteLine(new string('-', 20));
        }

        public int GetUserSelection()
        {
            Console.WriteLine(new string('-', 10));
            Console.WriteLine($"Enter number, please:");
            if (!int.TryParse(Console.ReadLine(), out var number))
                GetUserSelection();
            Console.WriteLine();
            return number;
        }
    }
}
