using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services;

public class MongoProjectsSevice : MongoService<Project>, IProjectsService
{
    protected override string collectionName => "projects";

    public MongoProjectsSevice(IConfiguration config) : base(config) { }

    public void Create(Project project) => collection.InsertOne(project);

    public List<Project> Get() => collection.Find(project => true).ToList();
}