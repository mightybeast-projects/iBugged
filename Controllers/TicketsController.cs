using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class TicketsController : ControllerBase
{
    private readonly TicketsService ticketsService;

    public TicketsController(TicketsService ticketsService) =>
        this.ticketsService = ticketsService;

    [HttpGet]
    public IActionResult List() =>
        View(nameof(List), ticketsService.GetTicketViewModels());

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.projectsList = ticketsService.GetProjectsList();
        ViewBag.usersList = ticketsService.GetUsersList();

        return View(nameof(Create));
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewBag.projectsList = ticketsService.GetProjectsList();
        ViewBag.usersList = ticketsService.GetUsersList();

        return View(nameof(Edit), ticketsService.Get(id));
    }

    [HttpGet]
    public IActionResult CloseTicket(string id)
    {
        ticketsService.CloseTicket(id);
        
        return RedirectToAction(nameof(List));
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        ticketsService.DeleteTicket(id);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Create(Ticket ticket)
    {
        ticketsService.CreateTicket(ticket);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Edit(Ticket ticket)
    {
        ticketsService.Edit(ticket.id, ticket);

        return RedirectToAction(nameof(List));
    }
}