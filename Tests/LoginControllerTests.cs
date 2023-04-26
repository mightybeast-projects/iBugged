using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class LoginControllerTests
{
    private Mock<IUsersService>? userServiceMock;
    private LoginController? loginController;

    [SetUp]
    public void SetUp()
    {
        userServiceMock = new Mock<IUsersService>();
        Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
        HttpSessionMock sessionMock = new HttpSessionMock();

        userServiceMock.Setup(m => m.Get("mightybeast@labs.com", "1")).Returns(user);
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

        loginController = new LoginController(userServiceMock.Object);
        loginController.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public void IndexCallbackReturnsIndexView()
    {
        var result = loginController!.Index();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("~/Views/Index.cshtml", ((ViewResult)result).ViewName);
    }

    [Test]
    public void RegisterGetCallbackReturnsRegisterView()
    {
        var result = loginController!.Register();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("Register", ((ViewResult)result).ViewName);
    }

    [Test]
    public void RegisterPostCallbackInsertsNewUserAndReturnsIndexView()
    {
        var result = loginController!.Register(newUser);

        userServiceMock!.Verify(m => m.Create(newUser));
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
    }

    [Test]
    public void LogInCallbackRedirectsToDashboardOnSuccessfulLogin()
    {
        var result = loginController!.LogIn("mightybeast@labs.com", "1");
        var session = loginController.ControllerContext.HttpContext.Session;
        var username = session.GetString("Username");

        Assert.IsInstanceOf<RedirectResult>(result);
        Assert.AreEqual("~/Dashboard", ((RedirectResult)result).Url);
        Assert.AreEqual(user.name, username);
    }

    [Test]
    public void LogInCallbackReturnsLoginViewOnFailedLogin()
    {
        var result = loginController!.LogIn("", "");

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("Incorect email or password",
            ((ViewResult)result).ViewData["ErrorMessage"]);
        Assert.AreEqual("Index", ((ViewResult)result).ViewName);
    }

    private User user = new User()
    {
        name = "MightyBeast",
        email = "mightybeast@labs.com",
        password = "1",
        organization = "MightyBeastLabs",
        role = "Project Manager"
    };

    private User newUser = new User()
    {
        name = "AnotherMightyBeast",
        email = "anothermightybeast@labs.com",
        password = "1",
        organization = "MightyBeastLabs",
        role = "Project Manager"
    };
}