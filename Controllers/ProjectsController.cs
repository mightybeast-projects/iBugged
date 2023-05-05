using iBugged.Models;
using iBugged.Services;
using iBugged.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class ProjectsController : Controller
{
    private readonly ProjectsService projectsService;

    public ProjectsController(ProjectsService projectService) =>
        this.projectsService = projectService;

    [HttpGet]
    public IActionResult List() =>
        View(nameof(List), GetProjectViewModels());

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.usersList = projectsService.GetUsersList();

        return View(nameof(Create));
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewBag.usersList = projectsService.GetUsersList();

        return View(nameof(Edit), projectsService.Get(id));
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        projectsService.DeleteProject(id);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
        projectsService.Create(project);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Edit(Project project)
    {
        projectsService.Edit(project.id, project);

        return RedirectToAction(nameof(List));
    }

    private List<ProjectViewModel> GetProjectViewModels()
    {
        string userJson = HttpContext.Session.GetString("User")!;
        User user = JsonConvert.DeserializeObject<User>(userJson)!;
        return projectsService.GetProjectViewModels(user);
    }
}