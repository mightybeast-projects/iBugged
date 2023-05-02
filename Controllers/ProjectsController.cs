using iBugged.Models;
using iBugged.ViewModels;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectsRepository projectsRepository;
    private readonly IUsersRepository usersRepository;

    public ProjectsController(
        IProjectsRepository projectsRepository,
        IUsersRepository usersRepository)
    {
        this.projectsRepository = projectsRepository;
        this.usersRepository = usersRepository;
    }

    [HttpGet]
    public IActionResult List() => View("List", GetProjectViewModels());

    [HttpGet]
    public IActionResult Create() =>
        View("Create", GetProjectCreationViewModel());

    [HttpGet]
    public IActionResult Delete(string id)
    {
        projectsRepository.Delete(id);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
        string userJson = HttpContext.Session.GetString("User")!;
        User user = JsonConvert.DeserializeObject<User>(userJson)!;

        project.membersId = new List<string>{ user.id };
        projectsRepository.Create(project);

        return RedirectToAction("List");
    }

    private List<ProjectViewModel> GetProjectViewModels()
    {
        List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();

        foreach (Project project in projectsRepository.Get())
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();
            projectViewModel.project = project;
            foreach (string memberId in project.membersId)
                projectViewModel.members.Add(usersRepository.Get(memberId));
            projectViewModels.Add(projectViewModel);
        }

        return projectViewModels;
    }

    private ProjectCreationViewModel GetProjectCreationViewModel() =>
        new ProjectCreationViewModel(){ users = usersRepository.Get() };
}