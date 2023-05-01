using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories;

public class MongoUsersRepository : MongoRepository<User>, IUsersRepository
{
    protected override string collectionName => "users";

    public MongoUsersRepository(IMongoDatabase db) : base(db) { }

    public User Get(string email, string password)
        => collection
            .Find(user => user.email == email && user.password == password)
            .FirstOrDefault();
}