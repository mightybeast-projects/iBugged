using System.Reflection.Metadata;
using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iBugged.Services;

public abstract class Service
{
    protected readonly IRepository<User> usersRepository;
    protected readonly IRepository<Project> projectsRepository;
    protected readonly IRepository<Ticket> ticketsRepository;

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
        List<SelectListItem> usersList = new List<SelectListItem>();
        usersRepository.GetAll()
            .ForEach(u => usersList
            .Add(new SelectListItem() { Text = u.name, Value = u.id }));
        return usersList;
    }

    public List<SelectListItem> GetProjectsList()
    {
        List<SelectListItem> projectsList = new List<SelectListItem>();
        projectsRepository.GetAll()
            .ForEach(p => projectsList
            .Add(new SelectListItem() { Text = p.name, Value = p.id }));
        return projectsList;
    }
}