using iBugged.Models;
using iBugged.Services.Repositories;
using iBugged.ViewModels;

namespace iBugged.Services;

public class TicketsService : Service
{
    private Project project = null!;

    public TicketsService(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
        : base(usersRepository, projectsRepository, ticketsRepository) { }

    public Ticket Get(string id) => ticketsRepository.Get(id);

    public void Edit(string id, Ticket ticket)
    {
        Project project = projectsRepository.Get(project => project.ticketsId.Contains(id));
        project.ticketsId.Remove(id);
        projectsRepository.Edit(project.id, project);

        ticketsRepository.Edit(id, ticket);

        project = projectsRepository.Get(ticket.projectId);
        project.ticketsId.Add(id);
        projectsRepository.Edit(project.id, project);
    }

    public void CloseTicket(string id) => ChangeTicketStatusAndEdit(id, false);

    public void ReopenTicket(string id) => ChangeTicketStatusAndEdit(id, true);

    public void DeleteTicket(string id)
    {
        project = projectsRepository.Get(project =>
            project.ticketsId.Contains(id));
        project.ticketsId.Remove(id);
        projectsRepository.Edit(project.id, project);
        ticketsRepository.Delete(id);
    }

    public void CreateTicket(Ticket ticket)
    {
        ticketsRepository.Create(ticket);
        project = projectsRepository.Get(ticket.projectId);
        project.ticketsId.Add(ticket.id);
        projectsRepository.Edit(project.id, project);
    }

    public List<TicketViewModel> GetTicketViewModels() =>
        ticketsRepository
            .GetAll()
            .ConvertAll(ticket => GetTicketViewModel(ticket));

    private void ChangeTicketStatusAndEdit(string id, bool status)
    {
        Ticket ticket = ticketsRepository.Get(id);
        ticket.isOpened = status;
        ticketsRepository.Edit(ticket.id, ticket);
    }

    private TicketViewModel GetTicketViewModel(Ticket ticket) =>
        new TicketViewModel()
        {
            ticket = ticket,
            project = projectsRepository.Get(ticket.projectId),
            assignee = usersRepository.Get(ticket.assigneeId),
            author = usersRepository.Get(ticket.authorId)
        };
}