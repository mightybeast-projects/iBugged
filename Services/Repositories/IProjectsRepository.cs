using iBugged.Models;

namespace iBugged.Services.Repositories;

public interface IProjectsRepository : IRepository<Project>
{
    Project Get(string id);
    void Edit(string id, Project project);
    void Delete(string id);
}