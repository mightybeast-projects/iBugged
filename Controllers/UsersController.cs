using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class UsersController : Controller
{
    private readonly UsersService usersService;

    public UsersController(UsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpGet]
    public IActionResult List() => View("List", usersService.Get());
}