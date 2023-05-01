using MongoDB.Driver;

namespace iBugged.Services.Repositories;

public abstract class MongoRepository<T> : IRepository<T>
{
    protected abstract string collectionName { get; }
    protected IMongoCollection<T> collection = null!;

    public MongoRepository(IMongoDatabase db) =>
        collection = db.GetCollection<T>(collectionName);

    public List<T> Get() => collection.Find(t => true).ToList();
    
    public void Create(T t) => collection.InsertOne(t);
}