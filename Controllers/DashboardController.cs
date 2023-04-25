using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index() => View("Home");
}
