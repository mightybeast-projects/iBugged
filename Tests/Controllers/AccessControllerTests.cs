using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class AccessControllerTests : ControllerTestsBase<AccessController>
{
    private const string SESSION_USER_STR = "User";
    private const string ERROR_MESSAGE_NAME = "ErrorMessage";
    private const string ERROR_MESSAGE = "Incorect email or password";
    private Mock<IUsersService> userServiceMock = null!;
    private readonly User user = TestsData.dummyUser;

    [SetUp]
    public void SetUp()
    {
        userServiceMock = new Mock<IUsersService>();
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();

        userServiceMock.Setup(m => m.Get(user.email, user.password))
            .Returns(user);
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

        controller = new AccessController(userServiceMock.Object);
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
    }

    [Test]
    public void LogInCallbackSetsUserInSessionOnSuccessfulLogin()
    {
        result = controller.LogIn(user.email, user.password);
        var json = sessionMock.GetString(SESSION_USER_STR);

        Assert.AreEqual(user, JsonConvert.DeserializeObject<User>(json!));
    }

    [Test]
    public void LogInCallbackReturnsLoginViewWithErrorMessageOnFailedLogin()
    {
        result = controller.LogIn(string.Empty, string.Empty);

        Assert.AreEqual(ERROR_MESSAGE,
            ((ViewResult)result).ViewData[ERROR_MESSAGE_NAME]);
        AssertViewResultReturnsViewWithName("Index");
    }

    [Test]
    public void LogOutCallbackReturnsIndexView()
    {
        result = controller.LogOut();

        AssertRedirectToActionResultReturnsActionWithName("Index");
    }

    [Test]
    public void LogOutCallBackClearsUserInSession()
    {
        var json = JsonConvert.SerializeObject(user);
        sessionMock.SetString(SESSION_USER_STR, json);

        result = controller.LogOut();

        Assert.Throws<KeyNotFoundException>(
            () => sessionMock.GetString(SESSION_USER_STR)
        );
    }
}