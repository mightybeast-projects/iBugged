using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services.Repositories;
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
    private Mock<IUsersRepository> userRepositoryMock = null!;
    private readonly User user = TestsData.dummyUser;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        userRepositoryMock = new Mock<IUsersRepository>();

        TestsData.users
            .ForEach(u => userRepositoryMock
            .Setup(m => m.Get(u.email, u.password))
            .Returns(u));

        controller = new AccessController(userRepositoryMock.Object);
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

        userRepositoryMock.Verify(m => m.Create(user));
        AssertRedirectToActionResultReturnsActionWithName("Index");
    }

    [Test, TestCaseSource(typeof(TestsData), nameof(TestsData.userCases))]
    public void LogInCallbackRedirectsToDashboardOnSuccessfulLoginOf(User user)
    {
        result = controller.LogIn(user.email, user.password);

        AssertRedirectResultRedirectsToPath("~/Dashboard/Home");
    }

    [Test, TestCaseSource(typeof(TestsData), nameof(TestsData.userCases))]
    public void LogInCallbackSetsUserInSessionOnSuccessfulLogin(User user)
    {
        result = controller.LogIn(user.email, user.password);
        var loggedUserJson = JsonConvert.SerializeObject(user);
        var sessionUserJson = sessionMock.GetString(SESSION_USER_STR);

        Assert.AreEqual(loggedUserJson, sessionUserJson);
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

    [Test, TestCaseSource(typeof(TestsData), nameof(TestsData.userCases))]
    public void LogOutCallBackClearsUserInSession(User user)
    {
        var json = JsonConvert.SerializeObject(user);
        sessionMock.SetString(SESSION_USER_STR, json);

        result = controller.LogOut();

        Assert.Throws<KeyNotFoundException>(
            () => sessionMock.GetString(SESSION_USER_STR)
        );
    }
}