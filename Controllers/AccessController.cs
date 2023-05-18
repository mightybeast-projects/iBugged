using GoogleAuthentication.Services;
using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        string clientId = "493795471117-io9gpva1nr1mguegl9queu6s5571od4e.apps.googleusercontent.com";
        string url = "http://localhost:5227/Access/SignInGoogle";
        string googleSignInUrl = GoogleAuth.GetAuthUrl(clientId, url);
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
        User user = JsonConvert.DeserializeObject<User>(userProfile)!;
        user.password = user.id;

        return LogIn(user.email, user.password);
    }

    [HttpGet]
    public IActionResult Register()
    {
        string clientId = "493795471117-io9gpva1nr1mguegl9queu6s5571od4e.apps.googleusercontent.com";
        string url = "http://localhost:5227/Access/RegisterGoogle";
        string googleSignInUrl = GoogleAuth.GetAuthUrl(clientId, url);
        ViewBag.googleSignInUrl = googleSignInUrl;

        return View(nameof(Register));
    }

    [HttpGet]
    public async Task<IActionResult> RegisterGoogle(string code)
    {
        var clientSecret = "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";
        var clientId = "493795471117-io9gpva1nr1mguegl9queu6s5571od4e.apps.googleusercontent.com";
        var url = "http://localhost:5227/Access/RegisterGoogle";
        var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientSecret, url);
        var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken);
        User user = JsonConvert.DeserializeObject<User>(userProfile)!;
        user.password = user.id;

        return View(nameof(RegisterGoogle), user);
    }

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