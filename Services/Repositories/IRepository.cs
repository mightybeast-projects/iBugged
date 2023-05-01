namespace iBugged.Services.Repositories;

public interface IRepository<T>
{
    List<T> Get();
    void Create(T t);
}