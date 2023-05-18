using GoogleAuthentication.Services;
using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iBugged.Controllers;

public class AccessController : ControllerBase
{
    private const string ERROR_MESSAGE = "Incorect email or password";
    private readonly IRepository<User> usersRepository;

    public AccessController(IRepository<User> usersRepository) =>
        this.usersRepository = usersRepository;

    [HttpGet]
    public IActionResult Index()
    {
        var clientId = "493795471117-io9gpva1nr1mguegl9queu6s5571od4e.apps.googleusercontent.com";
        var url = "http://localhost:5227/Access/SignInGoogle";
        var googleSignInUrl = GoogleAuth.GetAuthUrl(clientId, url);
        ViewBag.googleSignInUrl = googleSignInUrl;

        return View(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> SignInGoogle(string code)
    {
        var clientSecret = "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";
        var clientId = "493795471117-io9gpva1nr1mguegl9queu6s5571od4e.apps.googleusercontent.com";
        var url = "http://localhost:5227/Access/SignInGoogle";
        var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientSecret, url);
        var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken);
        ViewBag.userProfile = userProfile;

        return View(nameof(Index));
    }

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
        {
            SetUserInSession(user);
            return RedirectPermanent("~/Dashboard/Home");
        }

        ViewBag.errorMessage = ERROR_MESSAGE;

        return View(nameof(Index));
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        usersRepository.Create(user);

        return RedirectToAction(nameof(Index));
    }
}