using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : Controller
{
    private readonly IRepository<User> usersRepository;
    private readonly IRepository<Project> projectsRepository;
    private readonly IRepository<Ticket> ticketsRepository;

    public UsersController(
        IRepository<User> usersService,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
    {
        this.usersRepository = usersService;
        this.projectsRepository = projectsRepository;
        this.ticketsRepository = ticketsRepository;
    }

    [HttpGet]
    public IActionResult List() => View(nameof(List), usersRepository.GetAll());

    [HttpGet]
    public IActionResult Edit(string id) => View(nameof(Edit), usersRepository.Get(id));

    [HttpGet]
    public IActionResult Delete(string id)
    {
        List<Project> projects = 
            projectsRepository.GetAll(project =>
                project.membersId.Contains(id));
        projects.ForEach(project => project.membersId.Remove(id));
        projects.ForEach(project =>
            projectsRepository.Edit(project.id, project));

        List<Ticket> tickets = ticketsRepository.GetAll(ticket =>
            ticket.authorId == id);
        tickets.ForEach(ticket => ticket.authorId = null!);
        tickets.ForEach(ticket => ticketsRepository.Edit(ticket.id, ticket));
        
        tickets = ticketsRepository.GetAll(ticket =>
            ticket.assigneeId == id);
        tickets.ForEach(ticket => ticket.assigneeId = null!);
        tickets.ForEach(ticket => ticketsRepository.Edit(ticket.id, ticket));

        usersRepository.Delete(id);
        
        return RedirectToAction(nameof(List));
    }

    [HttpPost]
    public IActionResult Edit(User user)
    {
        usersRepository.Edit(user.id, user);

        return RedirectToAction(nameof(List));
    }
}