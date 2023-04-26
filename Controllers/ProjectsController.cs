using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class ProjectsController : Controller
{
    [HttpGet]
    public IActionResult List() => View("List");

    [HttpGet]
    public IActionResult Create() => View("Create");
}