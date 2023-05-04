using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class DashboardController : Controller
{
    [HttpGet]
    public IActionResult Home() => View(nameof(Home));
}
