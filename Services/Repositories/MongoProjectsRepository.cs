using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories;

public class MongoProjectsRepository : MongoRepository<Project>, IProjectsRepository
{
    protected override string collectionName => "projects";
    
    public MongoProjectsRepository(IMongoDatabase db) : base(db) { }

    public Project Get(string id) =>
        collection.Find(project => project.id == id).FirstOrDefault();

    public void Delete(string id) =>
        collection.DeleteOne(project => project.id == id);
}