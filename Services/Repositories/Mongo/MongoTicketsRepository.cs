using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories.Mongo;

public class MongoTicketsRepository : MongoRepository<Ticket>
{
    protected override string collectionName => "tickets";

    public MongoTicketsRepository(IMongoDatabase db) : base(db) { }
}