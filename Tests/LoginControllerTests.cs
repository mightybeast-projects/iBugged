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
    private Mock<IUsersService> userServiceMock = null!;
    private Mock<HttpContext> httpContextMock = null!;
    private HttpSessionMock sessionMock = null!;
    private LoginController controller = null!;

    [SetUp]
    public void SetUp()
    {
        userServiceMock = new Mock<IUsersService>();
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();

        userServiceMock.Setup(m => m.Get("mightybeast@labs.com", "1")).Returns(user);
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

        controller = new LoginController(userServiceMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public void IndexCallbackReturnsIndexView()
    {
        var result = controller.Index();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("~/Views/Index.cshtml", ((ViewResult)result).ViewName);
    }

    [Test]
    public void RegisterGetCallbackReturnsRegisterView()
    {
        var result = controller.Register();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("Register", ((ViewResult)result).ViewName);
    }

    [Test]
    public void RegisterPostCallbackInsertsNewUserAndReturnsIndexView()
    {
        var result = controller.Register(newUser);

        userServiceMock!.Verify(m => m.Create(newUser));
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
    }

    [Test]
    public void LogInCallbackRedirectsToDashboardOnSuccessfulLogin()
    {
        var result = controller.LogIn("mightybeast@labs.com", "1");
        var session = controller.ControllerContext.HttpContext.Session;
        var username = session.GetString("Username");

        Assert.IsInstanceOf<RedirectResult>(result);
        Assert.AreEqual("~/Dashboard/Home", ((RedirectResult)result).Url);
        Assert.AreEqual(user.name, username);
    }

    [Test]
    public void LogInCallbackReturnsLoginViewOnFailedLogin()
    {
        var result = controller.LogIn("", "");

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("Incorect email or password",
            ((ViewResult)result).ViewData["ErrorMessage"]);
        Assert.AreEqual("Index", ((ViewResult)result).ViewName);
    }

    [Test]
    public void LogOutCallbackClearsUsernameInSessionAndReturnIndexView()
    {
        controller!.HttpContext.Session.SetString("Username", user.name!);        

        var result = controller.LogOut();
        var session = controller.HttpContext.Session;

        Assert.IsInstanceOf<RedirectToActionResult>(result);
        Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        Assert.Throws<KeyNotFoundException>(
            () => session.GetString("Username")
        );
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