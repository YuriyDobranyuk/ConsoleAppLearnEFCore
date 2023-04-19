using System.Linq.Expressions;

namespace ConsoleAppLearnEFCore.Interface
{
    public interface IServiceLibrary
    {
        Task Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        T Get<T>(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        Task<IList<T>> GetAllItems<T>(params Expression<Func<T, object>>[] includes) where T : class;
        bool CheckExist<T>(T entity) where T : class;
        bool CheckExistByName<T>(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes) where T : class;
    }
}
