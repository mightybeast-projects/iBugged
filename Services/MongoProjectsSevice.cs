using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services;

public class MongoProjectsSevice : IProjectsService
{
    private const string DB_NAME = "iBugged_db";
    private const string COLLECTION_NAME = "projects";
    private IMongoCollection<Project> projects;

    public MongoProjectsSevice(IConfiguration config)
    {
        MongoClient client =
            new MongoClient(config.GetConnectionString(DB_NAME));
        IMongoDatabase db = client.GetDatabase(DB_NAME);
        projects = db.GetCollection<Project>(COLLECTION_NAME);
    }

    public void Create(Project project) => projects.InsertOne(project);
}