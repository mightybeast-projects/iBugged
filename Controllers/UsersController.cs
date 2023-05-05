using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : Controller
{
    private readonly IRepository<User> usersRepository;
    private readonly IRepository<Project> projectsRepository;

    public UsersController(
        IRepository<User> usersService,
        IRepository<Project> projectsRepository)
    {
        this.usersRepository = usersService;
        this.projectsRepository = projectsRepository;
    }

    [HttpGet]
    public IActionResult List() => View(nameof(List), usersRepository.GetAll());

    [HttpGet]
    public IActionResult Delete(string id)
    {
        List<Project> projects = 
            projectsRepository.GetAll(project =>
                project.membersId.Contains(id));
        projects.ForEach(project => project.membersId.Remove(id));
        projects.ForEach(project =>
            projectsRepository.Edit(project.id, project));
        usersRepository.Delete(id);
        
        return RedirectToAction(nameof(List));
    }
}