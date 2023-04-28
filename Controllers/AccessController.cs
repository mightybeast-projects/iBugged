using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class AccessController : Controller
{
    private readonly IUsersService usersService;

    public AccessController(IUsersService usersService) =>
        this.usersService = usersService;

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet]
    public IActionResult Register() => View("Register");

    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult LogIn(string email, string password)
    {
        User user = usersService.Get(email, password);
        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.name);
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
            return RedirectPermanent("~/Dashboard/Home");
        }

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