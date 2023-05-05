using System.Linq.Expressions;
using iBugged.Models;
using iBugged.Services.Repositories;

namespace iBugged.Services;

public class UsersService : Service
{
    private List<Project> projects = new List<Project>();
    private List<Ticket> tickets = new List<Ticket>();

    public UsersService(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
        : base(usersRepository, projectsRepository, ticketsRepository) { }

    public List<User> GetAll() => usersRepository.GetAll();

    public User Get(Expression<Func<User, bool>> e)
        => usersRepository.Get(e);

    public void Edit(string id, User user)
        => usersRepository.Edit(id, user);
    
    public void DeleteUser(string id)
    {
        projects = projectsRepository.GetAll(project =>
            project.membersId.Contains(id));
        projects.ForEach(project => project.membersId.Remove(id));
        projects.ForEach(project =>
            projectsRepository.Edit(project.id, project));

        tickets = ticketsRepository.GetAll(ticket =>
            ticket.authorId == id);
        tickets.ForEach(ticket => ticket.authorId = null!);
        tickets.ForEach(ticket =>
            ticketsRepository.Edit(ticket.id, ticket));
        
        tickets = ticketsRepository.GetAll(ticket =>
            ticket.assigneeId == id);
        tickets.ForEach(ticket => ticket.assigneeId = null!);
        tickets.ForEach(ticket =>
            ticketsRepository.Edit(ticket.id, ticket));

        usersRepository.Delete(id);
    }
}