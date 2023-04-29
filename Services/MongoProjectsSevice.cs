using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services;

public class MongoProjectsSevice : MongoService<Project>, IProjectsService
{
    protected override string collectionName => "projects";
    
    public MongoProjectsSevice(IMongoDatabase db) : base(db) { }

    public void Delete(string id) =>
        collection.DeleteOne(project => project.id == id);
}