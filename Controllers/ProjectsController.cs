using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

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
        project.members = new List<string>();
        project.members.Add(HttpContext.Session.GetString("Username")!);
        projectsService.Create(project);

        return RedirectToAction("List");
    }
}