using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class ProjectsController : Controller
{
    private IProjectsRepository projectsService;

    public ProjectsController(IProjectsRepository projectsService) =>
        this.projectsService = projectsService;

    [HttpGet]
    public IActionResult List() => View("List", projectsService.Get());

    [HttpGet]
    public IActionResult Create() => View("Create");

    [HttpGet]
    public IActionResult Delete(string id)
    {
        projectsService.Delete(id);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
        string userJson = HttpContext.Session.GetString("User")!;
        User user = JsonConvert.DeserializeObject<User>(userJson)!;

        project.membersId = new List<string>{ user.id };
        projectsService.Create(project);

        return RedirectToAction("List");
    }
}