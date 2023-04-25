using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class LoginController : Controller
{
    private readonly UsersService usersService;

    public LoginController(UsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet]
    public IActionResult Register() => View("Register");

    [HttpPost]
    public IActionResult Register(User user)
    {
        usersService.Create(user);

        return RedirectToAction(nameof(Index));
    }
}