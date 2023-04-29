using MongoDB.Driver;

namespace iBugged.Services;

public abstract class MongoService<T> : IService<T>
{
    protected abstract string collectionName { get; }
    protected IMongoCollection<T> collection = null!;

    public MongoService(IMongoDatabase db) =>
        collection = db.GetCollection<T>(collectionName);

    public List<T> Get() => collection.Find(t => true).ToList();
    
    public void Create(T t) => collection.InsertOne(t);
}