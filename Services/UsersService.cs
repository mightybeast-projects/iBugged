using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services;

public class UsersService
{
    private const string DB_NAME = "iBugged_db";
    private const string COLLECTION_NAME = "users";
    private IMongoCollection<User> users;

    public UsersService(IConfiguration config)
    {
        MongoClient client =
            new MongoClient(config.GetConnectionString(DB_NAME));
        IMongoDatabase db = client.GetDatabase(DB_NAME);
        users = db.GetCollection<User>(COLLECTION_NAME);
    }

    public List<User> Get() => users.Find(user => true).ToList();

    public void Create(User user) => users.InsertOne(user);
}