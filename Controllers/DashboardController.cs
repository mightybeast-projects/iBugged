using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class DashboardController : ControllerBase
{
    private readonly DashboardService dashboardService;

    public DashboardController(DashboardService dashboardService) =>
        this.dashboardService = dashboardService;

    [HttpGet]
    public IActionResult Home() =>
        View(nameof(Home),
            dashboardService.GetTicketViewModels(GetUserInSession().id));

    [HttpGet]
    public IActionResult CloseTicket(string id)
    {
        dashboardService.CloseTicket(id);
        
        return RedirectToAction(nameof(Home));
    }
}
