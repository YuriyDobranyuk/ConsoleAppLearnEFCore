using ConsoleAppLearnEFCore.Interface;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppLearnEFCore.Menu
{
    public class AuthorMenu : BaseMenu
    {
        public EventCallback BackToLibraryMenu { get; set; }
        public AuthorMenu(IServiceLibrary serviceLibrary) : base(serviceLibrary) { }
    }
}
