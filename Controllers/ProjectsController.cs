using iBugged.Models;
using iBugged.ViewModels;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    public IActionResult Edit(string id)
    {
        List<SelectListItem> usersList = new List<SelectListItem>();
        usersRepository.Get()
            .ForEach(u => usersList
            .Add(new SelectListItem() { Text = u.name, Value = u.id }));

        ViewBag.usersList = usersList;
        return View("Edit", projectsRepository.Get(id));
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        projectsRepository.Delete(id);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
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
            project.membersId.ForEach(id =>
                projectViewModel.members.Add(usersRepository.Get(id)));
            projectViewModels.Add(projectViewModel);
        }

        return projectViewModels;
    }

    private ProjectCreationViewModel GetProjectCreationViewModel()
    {
        ProjectCreationViewModel projectCreationVM
            = new ProjectCreationViewModel();

        usersRepository.Get()
            .ForEach(u => projectCreationVM.users
            .Add(new SelectListItem() { Text = u.name, Value = u.id }));

        return projectCreationVM;
    }
}