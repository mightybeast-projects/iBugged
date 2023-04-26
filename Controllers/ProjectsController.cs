using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class ProjectsController : Controller
{
    public IActionResult List() => View("List");
}