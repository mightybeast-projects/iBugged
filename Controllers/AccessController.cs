using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged.Controllers;

public class AccessController : Controller
{
    private readonly IUsersRepository usersRepository;

    public AccessController(IUsersRepository usersService) =>
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
        User user = usersRepository.Get(email, password);
        if (user != null)
            return SetUserInSessionAndRedirectToDashboard(user);

        ViewData["ErrorMessage"] = "Incorect email or password";

        return View("Index");
    }

    [HttpGet]
    public IActionResult LogInAsProjectManager() =>
        LogIn("demoprojectmanager@gmail.com", "1234567");

    [HttpGet]
    public IActionResult LogInAsDeveloper() =>
        LogIn("demodeveloper@gmail.com", "1234567");

    [HttpGet]
    public IActionResult LogInAsTeamMember() =>
        LogIn("demoteammember@gmail.com", "1234567");

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