public interface IService<T>
{
    List<T> Get();
    void Create(T t);
}