using iBugged.Models;

namespace iBugged.ViewModels;

public class ProjectCreationViewModel
{
    public Project project { get; set; } = null!;
    public List<User> users { get; set; } = new List<User>();  
}