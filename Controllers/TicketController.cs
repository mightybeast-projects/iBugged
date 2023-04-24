using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class TicketController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Get(int id, string name)
    {
        ViewData["Ticket"] = $"Ticket [ ID : { id }, name : {name} ] ";

        return View();
    }
}