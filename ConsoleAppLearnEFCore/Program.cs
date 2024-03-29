﻿using ConsoleAppLearnEFCore.Interface;
using ConsoleAppLearnEFCore.Menu;
using ConsoleAppLearnEFCore.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppLearnEFCore
{
    public class Program
    {
        private static ServiceCollection _services = new();
        static async Task Main(string[] args)
        {
            AddServices();

            var serviceProvider = _services.BuildServiceProvider();
            var service = serviceProvider.GetService<IServiceLibrary>();

            LibraryMenu libraryMenu = new LibraryMenu(service);
            await libraryMenu.ShowLibraryMenu();
            

            Console.ReadLine();
        }
        private static void AddServices()
        {
            _services.AddDbContext<ApplicationDbContext>();
            _services.AddScoped<IServiceLibrary, ServiceLibrary>();
            
        }
    }
    
}