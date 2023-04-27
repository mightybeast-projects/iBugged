using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services;

public class MongoUsersService : MongoService<User>, IUsersService
{
    protected override string collectionName => "users";

    public MongoUsersService(IMongoDatabase db) : base(db) { }

    public User Get(string email, string password)
        => collection
            .Find(user => user.email == email && user.password == password)
            .FirstOrDefault();
}