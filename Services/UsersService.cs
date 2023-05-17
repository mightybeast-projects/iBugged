using iBugged.Models;
using iBugged.Services.Repositories;

namespace iBugged.Services;

public class UsersService : Service
{
    public UsersService(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
        : base(usersRepository, projectsRepository, ticketsRepository) { }
    
    public List<User> GetAll() => usersRepository.GetAll();

    public User Get(string id) => usersRepository.Get(id);

    public void Edit(string id, User user) => usersRepository.Edit(id, user);
    
    public void DeleteUser(string id)
    {
        DeleteUserFromProjects(id);
        DeleteUserFromTicketsAuthor(id);
        DeleteUserFromTicketsAssignee(id);

        usersRepository.Delete(id);
    }

    private void DeleteUserFromProjects(string id) =>
        projectsRepository
            .GetAll(project => project.membersId.Contains(id))
            .ForEach(project =>
            {
                project.membersId.Remove(id);
                projectsRepository.Edit(project.id, project);
            });

    private void DeleteUserFromTicketsAuthor(string id) =>
        ticketsRepository
            .GetAll(ticket => ticket.authorId == id)
            .ForEach(ticket =>
            {
                ticket.authorId = null!;
                ticketsRepository.Edit(ticket.id, ticket);
            });

    private void DeleteUserFromTicketsAssignee(string id) =>
        ticketsRepository
            .GetAll(ticket => ticket.assigneeId == id)
            .ForEach(ticket =>
            {
                ticket.assigneeId = null!;
                ticketsRepository.Edit(ticket.id, ticket);
            });
}