using System.Linq.Expressions;
using iBugged.Models.Mongo;
using MongoDB.Driver;

namespace iBugged.Services.Repositories;

public class MongoRepository<T> : IRepository<T>
    where T : Document
{
    protected readonly IMongoCollection<T> collection;

    public MongoRepository(IMongoDatabase db) =>
        collection = db.GetCollection<T>(GetCollectionName());

    public List<T> GetAll() => collection.Find(t => true).ToList();

    public List<T> GetAll(Expression<Func<T, bool>> filter) =>
        collection.Find(filter).ToList();

    public T Get(string id) =>
        collection.Find(t => t.id == id).FirstOrDefault();

    public T Get(Expression<Func<T, bool>> filter) =>
        collection.Find(filter).FirstOrDefault();
    
    public void Create(T t) => collection.InsertOne(t);

    public void Edit(string id, T t) =>
        collection.ReplaceOne(t => t.id == id, t);

    public void Delete(string id) =>
        collection.DeleteOne(t => t.id == id);

    private string GetCollectionName() =>
        (typeof(T)
        .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
        .FirstOrDefault() as BsonCollectionAttribute)!
        .collectionName;
}