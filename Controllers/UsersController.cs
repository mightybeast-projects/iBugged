using iBugged.Models;
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

    [HttpGet]
    public IActionResult Insert() => View("Insert");

    [HttpPost]
    public IActionResult Insert(User user)
    {
        usersService.Create(user);

        return RedirectToAction(nameof(List));
    }
}