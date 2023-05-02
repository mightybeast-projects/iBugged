using iBugged.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iBugged.ViewModels;

public class ProjectCreationViewModel
{
    public Project project { get; set; } = null!;
    public List<SelectListItem> users { get; set; }
        = new List<SelectListItem>();
}