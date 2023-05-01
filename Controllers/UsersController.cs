using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : Controller
{
    private readonly IUsersRepository usersService;

    public UsersController(IUsersRepository usersService) =>
        this.usersService = usersService;

    [HttpGet]
    public IActionResult List() => View("List", usersService.Get());
}