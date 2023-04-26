using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class LoginController : Controller
{
    private readonly IUsersService usersService;

    public LoginController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpGet]
    public IActionResult Index() => View("~/Views/Index.cshtml");

    [HttpGet]
    public IActionResult Register() => View("Register");

    [HttpPost]
    public IActionResult LogIn(string email, string password)
    {
        User user = usersService.Get(email, password);
        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.name!);
            return RedirectPermanent("~/Dashboard");
        }

        ViewData["ErrorMessage"] = "Incorect email or password";

        return View("Index");
    }

    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        usersService.Create(user);

        return RedirectToAction(nameof(Index));
    }
}