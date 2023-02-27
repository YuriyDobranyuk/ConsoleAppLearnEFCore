using ConsoleAppLearnEFCore.Manager;

namespace ConsoleAppLearnEFCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            LibraryManager libraryManager = new LibraryManager();
            libraryManager.ShowMenuLibrary();
            
            Console.ReadLine();
        }
    }
    
}