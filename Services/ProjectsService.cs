using iBugged.Models;
using iBugged.Services.Repositories;
using iBugged.ViewModels;

namespace iBugged.Services;

public class ProjectsService : Service
{
    private Project project = null!;

    public ProjectsService(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
        : base(usersRepository, projectsRepository, ticketsRepository) { }

    public Project Get(string id) => projectsRepository.Get(id);

    public void Create(Project project) =>
        projectsRepository.Create(project);

    public void Edit(string id, Project project) =>
        projectsRepository.Edit(id, project);

    public void DeleteProject(string id)
    {
        project = projectsRepository.Get(id);
        projectsRepository.Delete(id);
        project.ticketsId.ForEach(id => ticketsRepository.Delete(id));
    }

    public List<ProjectViewModel> GetProjectViewModels(User user)
    {
        List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();
        List<Project> projects =
            projectsRepository.GetAll(project => 
                project.membersId.Contains(user.id));

        foreach (Project project in projects)
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();
            projectViewModel.project = project;
            project.membersId.ForEach(id =>
                projectViewModel.members.Add(usersRepository.Get(id)));
            project.ticketsId.ForEach(id =>
                projectViewModel.tickets.Add(ticketsRepository.Get(id)));
            projectViewModels.Add(projectViewModel);
        }

        return projectViewModels;
    }
}