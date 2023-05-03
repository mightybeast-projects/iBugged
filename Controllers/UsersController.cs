using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : Controller
{
    private readonly IRepository<User> usersRepository;

    public UsersController(IRepository<User> usersService) =>
        this.usersRepository = usersService;

    [HttpGet]
    public IActionResult List() => View("List", usersRepository.GetAll());
}