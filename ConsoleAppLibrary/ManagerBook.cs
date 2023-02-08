using ConsoleAppLibrary.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppLibrary
{
    public class ManagerBook
    {
        public ApplicationDbContext dbContext = new ApplicationDbContext();
        public Book currentBook { get; set; }
        public void AddNewBook()
        {
            dbContext.Books.Add(currentBook);
            dbContext.SaveChanges();
        }
    }
}
