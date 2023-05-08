using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class AccessController : ControllerBase
{
    private const string ERROR_MESSAGE = "Incorect email or password";
    private readonly IRepository<User> usersRepository;

    public AccessController(IRepository<User> usersService) =>
        this.usersRepository = usersService;

    [HttpGet]
    public IActionResult Index() => View(nameof(Index));

    [HttpGet]
    public IActionResult Register() => View(nameof(Register));

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
        if (user is not null)
            return SetUserInSessionAndRedirectToDashboard(user);

        ViewBag.errorMessage = ERROR_MESSAGE;

        return View(nameof(Index));
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        usersRepository.Create(user);

        return RedirectToAction(nameof(Index));
    }

    private IActionResult SetUserInSessionAndRedirectToDashboard(User user)
    {
        SetUserInSession(user);
        return RedirectPermanent("~/Dashboard/Home");
    }
}