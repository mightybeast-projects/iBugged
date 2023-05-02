using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class TicketsController : Controller
{
    [HttpGet]
    public IActionResult List()
    {
        return View("List");
    }
}