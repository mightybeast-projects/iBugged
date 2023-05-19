using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class AccessController : ControllerBase
{
    private const string WRONG_CREDENTIALS_ERROR_MESSAGE =
        "Incorect email or password.";
    private readonly AccessService accessService;

    public AccessController(AccessService accessService) =>
        this.accessService = accessService;

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.googleSignInUrl = accessService.GetGoogleSignInUrl();

        return View(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> SignInGoogle(string code)
    {
        User user = await accessService.GetGoogleUserForSignIn(code);

        return LogIn(user.email, user.password);
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewBag.googleRegisterUrl = accessService.GetGoogleRegisterUrl();

        return View(nameof(Register));
    }

    [HttpGet]
    public async Task<IActionResult> RegisterGoogle(string code)
    {
        User user = await accessService.GetGoogleUserForRegister(code);
        if (accessService.Get(user.email, user.password) is not null)
            return RedirectToAction(nameof(Index));

        return View(nameof(RegisterGoogle), user);
    }

    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult LogInAsProjectManager() =>
        LogIn("demoprojectmanager@gmail.com", "1234567");

    [HttpPost]
    public IActionResult LogInAsDeveloper() =>
        LogIn("demodeveloper@gmail.com", "1234567");

    [HttpPost]
    public IActionResult LogInAsTeamMember() =>
        LogIn("demoteammember@gmail.com", "1234567");

    [HttpPost]
    public IActionResult LogIn(string email, string password)
    {
        User user = accessService.Get(email, password);
        if (user is not null)
        {
            SetUserInSession(user);
            return RedirectPermanent("~/Dashboard/Home");
        }

        ViewBag.errorMessage = WRONG_CREDENTIALS_ERROR_MESSAGE;

        return Index();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        accessService.Create(user);

        return RedirectToAction(nameof(Index));
    }
}