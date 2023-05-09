using iBugged.Models;
using iBugged.Services.Repositories;
using iBugged.ViewModels;

namespace iBugged.Services;

public class ProjectsService : Service
{
    private ProjectViewModel projectVM = null!;

    public ProjectsService(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
        : base(usersRepository, projectsRepository, ticketsRepository) { }

    public Project Get(string id) => projectsRepository.Get(id);

    public void Create(Project project) => projectsRepository.Create(project);

    public void Edit(string id, Project project) =>
        projectsRepository.Edit(id, project);

    public void DeleteProject(string id)
    {
        Project project = projectsRepository.Get(id);
        projectsRepository.Delete(id);
        project.ticketsId.ForEach(id => ticketsRepository.Delete(id));
    }

    public List<ProjectViewModel> GetProjectViewModels(User user) =>
        projectsRepository
            .GetAll(project => project.membersId.Contains(user.id))
            .ConvertAll(project => GetProjectViewModel(project));

    private ProjectViewModel GetProjectViewModel(Project project)
    {
        projectVM = new ProjectViewModel();
        projectVM.project = project;
        project.membersId.ForEach(id =>
            projectVM.members.Add(usersRepository.Get(id)));
        project.ticketsId.ForEach(id =>
            projectVM.tickets.Add(ticketsRepository.Get(id)));
        return projectVM;
    }
}