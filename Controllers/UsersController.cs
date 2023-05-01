using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : Controller
{
    private readonly IUsersRepository usersRepository;

    public UsersController(IUsersRepository usersService) =>
        this.usersRepository = usersService;

    [HttpGet]
    public IActionResult List() => View("List", usersRepository.Get());
}