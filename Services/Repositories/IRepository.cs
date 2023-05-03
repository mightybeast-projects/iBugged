namespace iBugged.Services.Repositories;

public interface IRepository<T>
{
    T Get(string id);
    List<T> Get();
    void Create(T t);
}