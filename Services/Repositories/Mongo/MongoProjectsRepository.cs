using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories.Mongo;

public class MongoProjectsRepository
    : MongoRepository<Project>, IProjectsRepository
{
    protected override string collectionName => "projects";
    
    public MongoProjectsRepository(IMongoDatabase db) : base(db) { }
}