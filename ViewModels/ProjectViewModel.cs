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
        new [] {
            "rgb(255, 0, 0)", "rgb(54, 162, 235)",
            "rgb(235, 192, 0)", "rgb(194, 194, 194)"
        };

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
        new[] { "rgb(33, 196, 0)", "rgb(235, 192, 0)", "rgb(255, 0, 0)" };

    public int[] GetTicketPriorityCount() =>
        new[]
        {
            tickets.Count(ticket => ticket.priority == Priority.Low),
            tickets.Count(ticket => ticket.priority == Priority.Medium),
            tickets.Count(ticket => ticket.priority == Priority.High)
        };

    public string[] GetTicketStatuses() => new[] { "Closed", "Opened" };

    public string[] GetTicketStatusColors() =>
        new[] { "rgb(33, 196, 0)", "rgb(255, 0, 0)" };

    public int[] GetTicketStatusCount() =>
        new[]
        {
            tickets.Count(ticket => !ticket.isOpened),
            tickets.Count(ticket => ticket.isOpened)
        };
}