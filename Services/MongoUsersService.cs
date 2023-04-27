using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services;

public class MongoUsersService : MongoService<User>, IUsersService
{
    protected override string collectionName => "users";

    public MongoUsersService(IConfiguration config) : base(config) { }

    public List<User> Get() => collection.Find(user => true).ToList();

    public User Get(string email, string password)
        => collection
            .Find(user => user.email == email && user.password == password)
            .FirstOrDefault();

    public void Create(User user) => collection.InsertOne(user);
}