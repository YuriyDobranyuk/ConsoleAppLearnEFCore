using ConsoleAppLearnEFCore.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ConsoleAppLearnEFCore.Service
{
    public class ServiceLibrary : IServiceLibrary
    {
        private readonly ApplicationDbContext _libraryDbContext;
       
        public ServiceLibrary(ApplicationDbContext context)
        {
            _libraryDbContext = context;
        }

        public async Task Add<T>(T entity) where T : class
        {
            await _libraryDbContext.Set<T>().AddAsync(entity);
            _libraryDbContext.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            _libraryDbContext.Set<T>().Remove(entity);
            _libraryDbContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : class
        {
            _libraryDbContext.Set<T>().Update(entity);
            _libraryDbContext.SaveChanges();
        }

        public T? Get<T>(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = _libraryDbContext.Set<T>();

            //if(predicate != null) query.Where<T>(predicate);
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            //return query.FirstOrDefault();
            return query.FirstOrDefault(predicate);
        }
        public async Task<IList<T>> GetAllItems<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = _libraryDbContext.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var res = await query.ToListAsync();
            return res;
        }

        public bool CheckExist<T>(T element) where T : class
        {
            return element != null ? true : false;
        }

        public bool CheckExistByName<T>(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return CheckExist<T>(Get<T>(predicate, includes));
        }

        
    }
}
