using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Manager;
using ConsoleAppLearnEFCore.Menu;
using ConsoleAppLearnEFCore.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppLearnEFCore
{
    public class Program
    {
        private static ServiceCollection _services = new();
        static void Main(string[] args)
        {
            AddServices();

            var serviceProvider = _services.BuildServiceProvider();
            var service = serviceProvider.GetService<ILibraryMenu>();
            if (service is not null)
            {
                service.ShowLibraryMenu();
            }

            

            Console.ReadLine();
        }
        private static void AddServices()
        {
            _services.AddDbContext<ApplicationDbContext>();
            _services.AddScoped<IServiceLibrary, ServiceLibrary>();
            _services.AddScoped<ILibraryMenu, LibraryMenu>();
            _services.AddScoped<ISectionMenu, SectionMenu>();
        }
    }
    
}