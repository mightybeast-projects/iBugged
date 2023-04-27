using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class LoginControllerTests : ControllerTestsBase<LoginController>
{
    private const string SESSION_USERNAME_STR = "Username";
    private const string ERROR_MESSAGE_NAME = "ErrorMessage";
    private const string ERROR_MESSAGE = "Incorect email or password";
    private Mock<IUsersService> userServiceMock = null!;
    private User user = TestsData.dummyUser;

    [SetUp]
    public void SetUp()
    {
        userServiceMock = new Mock<IUsersService>();
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();

        userServiceMock.Setup(m => m.Get(user.email, user.password))
            .Returns(user);
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

        controller = new LoginController(userServiceMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public void IndexCallbackReturnsIndexView()
    {
        result = controller.Index();

        AssertViewResultReturnsViewWithName("Index");
    }

    [Test]
    public void RegisterGetCallbackReturnsRegisterView()
    {
        result = controller.Register();

        AssertViewResultReturnsViewWithName("Register");
    }

    [Test]
    public void RegisterPostCallbackInsertsNewUserAndReturnsIndexView()
    {
        result = controller.Register(user);

        userServiceMock.Verify(m => m.Create(user));
        AssertRedirectToActionResultReturnsActionWithName("Index");
    }

    [Test]
    public void LogInCallbackRedirectsToDashboardOnSuccessfulLogin()
    {
        result = controller.LogIn(user.email, user.password);

        AssertRedirectResultRedirectsToPath("~/Dashboard/Home");
        Assert.AreEqual(user.name,
            sessionMock.GetString(SESSION_USERNAME_STR));
    }

    [Test]
    public void LogInCallbackReturnsLoginViewOnFailedLoginWithErrorMessage()
    {
        result = controller.LogIn(string.Empty, string.Empty);

        AssertViewResultReturnsViewWithName("Index");
        Assert.AreEqual(ERROR_MESSAGE,
            ((ViewResult)result).ViewData[ERROR_MESSAGE_NAME]);
    }

    [Test]
    public void LogOutCallbackClearsUsernameInSessionAndReturnsIndexView()
    {
        sessionMock.SetString(SESSION_USERNAME_STR, user.name);

        result = controller.LogOut();

        AssertRedirectToActionResultReturnsActionWithName("Index");
        Assert.Throws<KeyNotFoundException>(
            () => sessionMock.GetString(SESSION_USERNAME_STR)
        );
    }
}