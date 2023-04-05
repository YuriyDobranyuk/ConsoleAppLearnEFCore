using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Manager;
using ConsoleAppLearnEFCore.Model;
using Microsoft.AspNetCore.Components;
//using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAppLearnEFCore.Menu
{
    public class LibraryMenu : BaseMenu, ILibraryMenu
    {
        public SectionMenu SectionMenu { get; set; }
        public BookMenu BookMenu { get; set; }
        public AuthorMenu AuthorMenu { get; set; }
        
        public LibraryMenu(IServiceLibrary serviceLibrary)
        {
            BookMenu = new BookMenu(serviceLibrary)
            {
                BackToLibraryMenu = EventCallback.Factory.Create(this, () => ShowLibraryMenu())
            };
            SectionMenu = new SectionMenu(serviceLibrary)
            {
                BackToLibraryMenu = EventCallback.Factory.Create(this, () => ShowLibraryMenu()),
                //BackToChooseBooks = EventCallback.Factory.Create(this, (List<Book> books) => BookMenu.ChooseBooks(books))
            };
            
            BookMenu = new BookMenu(serviceLibrary)
            {
                BackToLibraryMenu = EventCallback.Factory.Create(this, () => ShowLibraryMenu())
            };
        }

        public async Task ShowLibraryMenu()
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
                    await SectionMenu.ShowMenuSectionLibrary();
                    break;
                case 2:
                    BookMenu.ShowMenuBookLibrary();
                    break;
                /*case 3:
                    AuthorManager.ShowMenuAuthorLibrary();
                    break;*/
            }
            EnterKeyForContinueWork();
            if (selection != 0) await ShowLibraryMenu();
        }
    }
}
