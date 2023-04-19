using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ConsoleAppLearnEFCore.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public int Pages { get; set; }
        public List<Section> BookSections { get; set; } = new List<Section>();
        public List<Author> BookAuthors { get; set; } = new List<Author>();

    }
}
