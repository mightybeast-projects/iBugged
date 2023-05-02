using iBugged.Models;

namespace iBugged.ViewModels;

public class ProjectViewModel
{
    public Project project { get; set; } = null!;
    public List<User> members { get; set; } = new List<User>();
}