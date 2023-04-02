using ConsoleAppLearnEFCore.Model;


namespace ConsoleAppLearnEFCore.Interface
{
    public interface IService<T> where T : class
    {
        void Add(T element); 
        bool CheckExist(T element);
        bool CheckExistByName(string name);
        void Edit();
        void Delete(T element);
        T GetByName(string name);
        List<T> GetAll();
        void Delete(Section element);
    }
}
