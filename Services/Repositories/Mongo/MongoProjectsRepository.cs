using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services.Repositories.Mongo;

public class MongoProjectsRepository : MongoRepository<Project>, IProjectsRepository
{
    protected override string collectionName => "projects";
    
    public MongoProjectsRepository(IMongoDatabase db) : base(db) { }

    public Project Get(string id) =>
        collection.Find(project => project.id == id).FirstOrDefault();

    public void Edit(string id, Project project) =>
        collection.ReplaceOne(project => project.id == id, project);

    public void Delete(string id) =>
        collection.DeleteOne(project => project.id == id);
}