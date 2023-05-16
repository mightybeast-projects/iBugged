using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class ProjectsController : ControllerBase
{
    private readonly ProjectsService projectsService;

    public ProjectsController(ProjectsService projectService) =>
        this.projectsService = projectService;

    [HttpGet]
    public IActionResult List() =>
        View(nameof(List), projectsService.GetProjectViewModels());

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.usersList = projectsService.GetUsersList();

        return PartialView(nameof(Create));
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewBag.usersList = projectsService.GetUsersList();

        return PartialView(nameof(Edit), projectsService.Get(id));
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        projectsService.DeleteProject(id);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult List(string searchString)
    {
        ViewData["SearchString"] = searchString;

        return View(nameof(List), projectsService.GetProjectViewModels(searchString));
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
}