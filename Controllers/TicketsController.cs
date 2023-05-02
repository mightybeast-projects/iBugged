using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class TicketsController : Controller
{
    private readonly ITicketsRepository ticketsRepository;

    public TicketsController(ITicketsRepository ticketsRepository)
        => this.ticketsRepository = ticketsRepository;

    [HttpGet]
    public IActionResult List() => View("List");

    [HttpGet]
    public IActionResult Create() => View("Create");

    [HttpPost]
    public IActionResult Create(Ticket ticket)
    {
        ticketsRepository.Create(ticket);

        return RedirectToAction(nameof(List));
    }
}