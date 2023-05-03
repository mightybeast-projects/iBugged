using System.Linq.Expressions;
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
    private Mock<IUsersRepository> userRepositoryMock;
    private readonly User user = TestsData.dummyUser;

    public AccessControllerTests()
    {
        userRepositoryMock = new Mock<IUsersRepository>();
        controller = new AccessController(userRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [SetUp]
    public new void SetUp()
    {
        TestsData.users
            .ForEach(u => userRepositoryMock
            .Setup(m => m.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns((Expression<Func<User, bool>> predicate) =>
                TestsData.users
                .Where(predicate.Compile())
                .FirstOrDefault()!));
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
    public void RegisterPostCallbackInsertsNewUser()
    {
        result = controller.Register(user);

        userRepositoryMock.Verify(m => m.Create(user));
    }

    [Test]
    public void RegisterPostCallbackReturnsIndexView()
    {
        result = controller.Register(user);

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