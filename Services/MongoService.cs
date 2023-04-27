using MongoDB.Driver;

namespace iBugged.Services;

public abstract class MongoService<T>
{
    protected abstract string collectionName { get; }
    protected IMongoCollection<T> collection = null!;
    private string dbName = "iBugged_db";

    public MongoService(IConfiguration config)
    {
        MongoClient client =
            new MongoClient(config.GetConnectionString(dbName));
        IMongoDatabase db = client.GetDatabase(dbName);
        collection = db.GetCollection<T>(collectionName);
    }
}