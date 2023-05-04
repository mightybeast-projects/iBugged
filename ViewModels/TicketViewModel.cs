using iBugged.Models;

namespace iBugged.ViewModels;

public class TicketViewModel
{
    public Ticket ticket { get; set; } = null!;
    public Project project { get; set; } = null!;
    public User assignee { get; set; } = null!;
    public User author { get; set; } = null!;
}