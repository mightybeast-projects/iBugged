using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iBugged.Controllers;

public class TicketsController : Controller
{
    private readonly IRepository<Ticket> ticketsRepository;
    private readonly IRepository<Project> projectsRepository;

    public TicketsController(
        IRepository<Ticket> ticketsRepository, 
        IRepository<Project> projectsRepository)
    {
        this.ticketsRepository = ticketsRepository;
        this.projectsRepository = projectsRepository;
    }

    [HttpGet]
    public IActionResult List() => View("List", ticketsRepository.GetAll());

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.projectsList = GetProjectsList();

        return View("Create");
    }

    [HttpPost]
    public IActionResult Create(Ticket ticket)
    {
        ticketsRepository.Create(ticket);

        return RedirectToAction(nameof(List));
    }

    private List<SelectListItem> GetProjectsList()
    {
        List<SelectListItem> projectsList = new List<SelectListItem>();
        projectsRepository.GetAll()
            .ForEach(p => projectsList
            .Add(new SelectListItem() { Text = p.name, Value = p.id }));
        return projectsList;
    }
}