using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories;

public class MongoProjectsRepository : MongoRepository<Project>, IProjectsRepository
{
    protected override string collectionName => "projects";
    
    public MongoProjectsRepository(IMongoDatabase db) : base(db) { }

    public void Delete(string id) =>
        collection.DeleteOne(project => project.id == id);
}