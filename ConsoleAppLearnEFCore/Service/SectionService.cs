using ConsoleAppLearnEFCore.Interface;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using ConsoleAppLearnEFCore.Model;
using Microsoft.AspNetCore.Components;
//using SectionLibrary = ConsoleAppLearnEFCore.Model.Section;

namespace ConsoleAppLearnEFCore.Service
{
    public class SectionService : BaseService
    {
        public void Add(Section element)
        {
            if (element != null)
            {
                dataBaseLibrary.Sections.Add(element);
                dataBaseLibrary.SaveChanges();
            }
        }

        public bool CheckExist(Section element)
        {
            return element != null ? true : false;
        }

        public bool CheckExistByName(string name)
        {
            return CheckExist(GetByName(name));
        }

        public void Delete(Section element)
        {
            if (element != null) dataBaseLibrary.Sections.Remove(element);
        }

        public void Edit()
        {
            throw new NotImplementedException();
        }

        public List<Section> GetAll()
        {
            return dataBaseLibrary.Sections.Include(section => section.BookSections)
                                           .ThenInclude(book => book.BookAuthors)
                                           .ToList();
        }

        Section GetByName(string name)
        {
            var findSection = dataBaseLibrary.Sections
                                             .Where(section => section.Name == name)
                                             .Include(section => section.BookSections)
                                             .ThenInclude(book => book.BookAuthors)
                                             .FirstOrDefault();
            return findSection;
        }
    }
}
