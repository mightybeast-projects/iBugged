using iBugged.Models;
using iBugged.Services.Repositories;
using iBugged.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iBugged.Controllers;

public class TicketsController : Controller
{
    private readonly IRepository<Ticket> ticketsRepository;
    private readonly IRepository<Project> projectsRepository;
    private readonly IRepository<User> usersRepository;

    public TicketsController(
        IRepository<Ticket> ticketsRepository, 
        IRepository<Project> projectsRepository,
        IRepository<User> usersRepository)
    {
        this.ticketsRepository = ticketsRepository;
        this.projectsRepository = projectsRepository;
        this.usersRepository = usersRepository;
    }

    [HttpGet]
    public IActionResult List() => View(nameof(List), GetTicketViewModels());

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.projectsList = GetProjectsList();
        ViewBag.usersList = GetUsersList();

        return View(nameof(Create));
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewBag.projectsList = GetProjectsList();
        ViewBag.usersList = GetUsersList();

        return View(nameof(Edit), ticketsRepository.Get(id));
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        Project project = projectsRepository.Get(project =>
            project.ticketsId.Contains(id));
        project.ticketsId.Remove(id);
        projectsRepository.Edit(project.id, project);
        ticketsRepository.Delete(id);
        
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Create(Ticket ticket)
    {
        Project project = projectsRepository.Get(ticket.projectId);
        project.ticketsId.Add(ticket.id);
        projectsRepository.Edit(project.id, project);
        ticketsRepository.Create(ticket);

        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Edit(Ticket ticket)
    {
        ticketsRepository.Edit(ticket.id, ticket);

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

    private List<SelectListItem> GetUsersList()
    {
        List<SelectListItem> usersList = new List<SelectListItem>();
        usersRepository.GetAll()
            .ForEach(u => usersList
            .Add(new SelectListItem() { Text = u.name, Value = u.id }));
        return usersList;
    }

    private List<TicketViewModel> GetTicketViewModels()
    {
        List<TicketViewModel> ticketViewModels = new List<TicketViewModel>();

        foreach(Ticket ticket in ticketsRepository.GetAll())
        {
            TicketViewModel ticketVM = new TicketViewModel();
            ticketVM.ticket = ticket;
            ticketVM.project = projectsRepository.Get(ticket.projectId);
            ticketVM.assignee = usersRepository.Get(ticket.assigneeId);
            ticketVM.author = usersRepository.Get(ticket.authorId);
            ticketViewModels.Add(ticketVM);
        }

        return ticketViewModels;
    }
}