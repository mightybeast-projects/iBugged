using System.Linq.Expressions;

namespace iBugged.Services.Repositories;

public interface IRepository<T>
{
    List<T> GetAll();
    List<T> GetAll(Expression<Func<T, bool>> filter);
    T Get(string id);
    T Get(Expression<Func<T, bool>> filter);
    void Create(T t);
    void Edit(string id, T t);
    void Delete(string id);
}