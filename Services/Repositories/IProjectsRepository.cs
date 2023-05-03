using iBugged.Models;

namespace iBugged.Services.Repositories;

public interface IProjectsRepository : IRepository<Project>
{
    void Edit(string id, Project project);
    void Delete(string id);
}