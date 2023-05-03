using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories.Mongo;

public class MongoUsersRepository
    : MongoRepository<User>, IUsersRepository
{
    protected override string collectionName => "users";

    public MongoUsersRepository(IMongoDatabase db) : base(db) { }
}