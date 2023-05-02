using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories;

public class MongoTicketsRepository : MongoRepository<Ticket>, ITicketsRepository
{
    protected override string collectionName => "tickets";

    public MongoTicketsRepository(IMongoDatabase db) : base(db) { }
}