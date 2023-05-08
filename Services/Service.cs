using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iBugged.Services;

public abstract class Service
{
    protected readonly IRepository<User> usersRepository;
    protected readonly IRepository<Project> projectsRepository;
    protected readonly IRepository<Ticket> ticketsRepository;
    private List<SelectListItem> itemList = null!;

    public Service(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
    {
        this.usersRepository = usersRepository;
        this.projectsRepository = projectsRepository;
        this.ticketsRepository = ticketsRepository;
    }

    public List<SelectListItem> GetUsersList()
    {
        itemList = new List<SelectListItem>();
        usersRepository.GetAll()
            .ForEach(u => itemList
            .Add(new SelectListItem() { Text = u.name, Value = u.id }));

        return itemList;
    }

    public List<SelectListItem> GetProjectsList()
    {
        itemList = new List<SelectListItem>();
        projectsRepository.GetAll()
            .ForEach(p => itemList
            .Add(new SelectListItem() { Text = p.name, Value = p.id }));

        return itemList;
    }
}