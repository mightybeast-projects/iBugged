using iBugged.Models;

namespace iBugged.Services;

public interface IProjectsService : IService<Project>
{
    void Delete(string id);
}