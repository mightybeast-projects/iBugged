using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories.Mongo;

public abstract class MongoRepository<T> : IRepository<T> where T : Document
{
    protected abstract string collectionName { get; }
    protected readonly IMongoCollection<T> collection;

    public MongoRepository(IMongoDatabase db) =>
        collection = db.GetCollection<T>(collectionName);

    public List<T> Get() => collection.Find(t => true).ToList();

    public T Get(string id) =>
        collection.Find(t => t.id == id).FirstOrDefault();
    
    public void Create(T t) => collection.InsertOne(t);
}