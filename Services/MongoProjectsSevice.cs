using iBugged.Models;
using MongoDB.Driver;

namespace iBugged.Services;

public class MongoProjectsSevice : MongoService<Project>, IProjectsService
{
    protected override string collectionName => "projects";
    
    public MongoProjectsSevice(IMongoDatabase db) : base(db) { }
}