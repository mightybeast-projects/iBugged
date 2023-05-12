using iBugged.Models;
using iBugged.Models.Enums;

namespace iBugged.ViewModels;

public class ProjectViewModel
{
    public Project project { get; set; } = null!;
    public List<User> members { get; set; } = new List<User>();
    public List<Ticket> tickets { get; set; } = new List<Ticket>();

    public string[] GetTicketTypes() =>
        System.Enum.GetNames(typeof(TicketType));

    public string[] GetTicketTypesColors() =>
        new [] { "#ff0000", "#9a00c9", "#00c3c9", "#969696" };

    public int[] GetTicketTypesCount() =>
        new[]
        {
            tickets.Count(ticket => ticket.ticketType == TicketType.Bug),
            tickets.Count(ticket => ticket.ticketType == TicketType.Task),
            tickets.Count(ticket => ticket.ticketType == TicketType.Support),
            tickets.Count(ticket => ticket.ticketType == TicketType.Other)
        };

    public string[] GetTicketPriorities() =>
        System.Enum.GetNames(typeof(Priority));

    public string[] GetTicketPriorityColors() =>
        new[] { "#00c900", "#e3cc00", "#ff0000" };

    public int[] GetTicketPriorityCount() =>
        new[]
        {
            tickets.Count(ticket => ticket.priority == Priority.Low),
            tickets.Count(ticket => ticket.priority == Priority.Medium),
            tickets.Count(ticket => ticket.priority == Priority.High)
        };

    public string[] GetTicketStatuses() => new[] { "Closed", "Opened" };

    public string[] GetTicketStatusColors() => new[] { "#00c900", "#ff0000" };

    public int[] GetTicketStatusCount() =>
        new[]
        {
            tickets.Count(ticket => !ticket.isOpened),
            tickets.Count(ticket => ticket.isOpened)
        };
}