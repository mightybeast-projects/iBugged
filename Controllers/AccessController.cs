using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class AccessController : Controller
{
    private readonly IRepository<User> usersRepository;

    public AccessController(IRepository<User> usersService) =>
        this.usersRepository = usersService;

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
        User user = usersRepository
            .Get(f => f.email == email && f.password == password);
        if (user != null)
            return SetUserInSessionAndRedirectToDashboard(user);

        ViewData["ErrorMessage"] = "Incorect email or password";

        return View("Index");
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        usersRepository.Create(user);

        return RedirectToAction(nameof(Index));
    }

    private IActionResult SetUserInSessionAndRedirectToDashboard(User user)
    {
        string json = JsonConvert.SerializeObject(user);
        HttpContext.Session.SetString("User", json);
        return RedirectPermanent("~/Dashboard/Home");
    }
}