using iBugged.Models;

namespace iBugged.Services.Repositories;

public interface IProjectsRepository : IRepository<Project>
{
    void Delete(string id);
}