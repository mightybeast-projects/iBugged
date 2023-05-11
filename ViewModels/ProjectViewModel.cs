using iBugged.Models;
using iBugged.Models.Enums;
using Newtonsoft.Json;

namespace iBugged.ViewModels;

public class ProjectViewModel
{
    public Project project { get; set; } = null!;
    public List<User> members { get; set; } = new List<User>();
    public List<Ticket> tickets { get; set; } = new List<Ticket>();

    public string GetTicketTypesCountJson() =>
        JsonConvert.SerializeObject(new[]
        {
            tickets.Count(ticket => ticket.ticketType == TicketType.Bug),
            tickets.Count(ticket => ticket.ticketType == TicketType.Task),
            tickets.Count(ticket => ticket.ticketType == TicketType.Support),
            tickets.Count(ticket => ticket.ticketType == TicketType.Other)
        });

    public string GetTicketPriorityCountJson() =>
        JsonConvert.SerializeObject(new[]
        {
            tickets.Count(ticket => ticket.priority == Priority.Low),
            tickets.Count(ticket => ticket.priority == Priority.Medium),
            tickets.Count(ticket => ticket.priority == Priority.High)
        });

    public string GetTicketStatusCountJson() =>
        JsonConvert.SerializeObject(new[]
        {
            tickets.Count(ticket => !ticket.isOpened),
            tickets.Count(ticket => ticket.isOpened)
        });
}