using iBugged.Models;
using iBugged.ViewModels;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iBugged.Controllers;

public class ProjectsController : Controller
{
    private readonly IRepository<Project> projectsRepository;
    private readonly IRepository<User> usersRepository;

    public ProjectsController(
        IRepository<Project> projectsRepository,
        IRepository<User> usersRepository)
    {
        this.projectsRepository = projectsRepository;
        this.usersRepository = usersRepository;
    }

    [HttpGet]
    public IActionResult List() => View("List", GetProjectViewModels());

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.usersList = GetUsersList();

        return View("Create");
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewBag.usersList = GetUsersList();

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

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Edit(Project project)
    {
        projectsRepository.Edit(project.id, project);

        return RedirectToAction(nameof(List));
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

    private List<SelectListItem> GetUsersList()
    {
        List<SelectListItem> usersList = new List<SelectListItem>();
        usersRepository.Get()
            .ForEach(u => usersList
            .Add(new SelectListItem() { Text = u.name, Value = u.id }));
        return usersList;
    }
}