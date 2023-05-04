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
    private readonly IRepository<Ticket> ticketsRepository;

    public ProjectsController(
        IRepository<Project> projectsRepository,
        IRepository<User> usersRepository,
        IRepository<Ticket> ticketsRepository)
    {
        this.projectsRepository = projectsRepository;
        this.usersRepository = usersRepository;
        this.ticketsRepository = ticketsRepository;
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
        string userJson = HttpContext.Session.GetString("User")!;
        User user = JsonConvert.DeserializeObject<User>(userJson)!;
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

    private List<SelectListItem> GetUsersList()
    {
        List<SelectListItem> usersList = new List<SelectListItem>();
        usersRepository.GetAll()
            .ForEach(u => usersList
            .Add(new SelectListItem() { Text = u.name, Value = u.id }));
        return usersList;
    }
}