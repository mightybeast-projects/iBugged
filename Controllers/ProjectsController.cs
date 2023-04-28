using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class ProjectsController : Controller
{
    private IProjectsService projectsService;

    public ProjectsController(IProjectsService projectsService) =>
        this.projectsService = projectsService;

    [HttpGet]
    public IActionResult List() => View("List", projectsService.Get());

    [HttpGet]
    public IActionResult Create() => View("Create");

    [HttpPost]
    public IActionResult Create(Project project)
    {
        string userJson = HttpContext.Session.GetString("User")!;
        User user = JsonConvert.DeserializeObject<User>(userJson)!;

        project.members = new List<string>();
        project.members.Add(user.name);
        projectsService.Create(project);

        return RedirectToAction("List");
    }
}