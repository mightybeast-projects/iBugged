using iBugged.Models;

namespace iBugged.Services;

public interface IProjectsService
{
    List<Project> Get();
    void Create(Project project);
}