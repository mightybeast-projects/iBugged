using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class DashboardController : Controller
{
    private readonly DashboardService dashboardService;

    public DashboardController(DashboardService dashboardService) =>
        this.dashboardService = dashboardService;

    [HttpGet]
    public IActionResult Home()
    {
        string userJson = HttpContext.Session.GetString("User")!;
        User user = JsonConvert.DeserializeObject<User>(userJson)!;

        return View(nameof(Home),
            dashboardService.GetTicketViewModels(user.id));
    }

    [HttpGet]
    public IActionResult CloseTicket(string id)
    {
        dashboardService.CloseTicket(id);
        
        return RedirectToAction(nameof(Home));
    }
}
