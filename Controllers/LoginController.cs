using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class LoginController : Controller
{
    private readonly IUsersService usersService;

    public LoginController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet]
    public IActionResult Register() => View("Register");

    [HttpPost]
    public IActionResult LogIn(string email, string password)
    {
        if (usersService.Get(email, password) != null)
            return RedirectPermanent("~/Dashboard");

        ViewData["ErrorMessage"] = "Incorect email or password";
        return View("Index");
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        usersService.Create(user);

        return RedirectToAction(nameof(Index));
    }
}