using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories.Mongo;

public class MongoProjectsRepository : MongoRepository<Project>
{
    protected override string collectionName => "projects";
    
    public MongoProjectsRepository(IMongoDatabase db) : base(db) { }
}