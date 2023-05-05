using iBugged.Models;
using iBugged.Services.Repositories;

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
}