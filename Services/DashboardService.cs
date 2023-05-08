using iBugged.Models;
using iBugged.Services.Repositories;
using iBugged.ViewModels;

namespace iBugged.Services;

public class DashboardService : Service
{
    public DashboardService(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
        : base(usersRepository, projectsRepository, ticketsRepository) { }

    public void CloseTicket(string id)
    {
        Ticket ticket = ticketsRepository.Get(id);
        ticket.isOpened = false;
        ticketsRepository.Edit(ticket.id, ticket);
    }

    public List<TicketViewModel> GetTicketViewModels(string userId)
    {
        List<TicketViewModel> ticketViewModels = new List<TicketViewModel>();

        ticketsRepository
            .GetAll(ticket =>
                ticket.assigneeId == userId && ticket.isOpened)
            .ForEach(ticket =>
                ticketViewModels.Add(GetTicketViewModel(ticket)));

        return ticketViewModels;
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