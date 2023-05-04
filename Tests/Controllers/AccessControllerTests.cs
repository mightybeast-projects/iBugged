using iBugged.Controllers;
using iBugged.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class AccessControllerTests : ControllerTestsBase<AccessController>
{
    private const string ERROR_MESSAGE_NAME = "ErrorMessage";
    private const string ERROR_MESSAGE = "Incorect email or password";

    public AccessControllerTests()
    {
        controller = new AccessController(usersRepositoryMock.Object);
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
    public void RegisterPostCallbackInsertsNewUser()
    {
        result = controller.Register(user);

        usersRepositoryMock.Verify(m => m.Create(user));
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

        var viewResult = (ViewResult)result;
        Assert.AreEqual(ERROR_MESSAGE,
            viewResult.ViewData[ERROR_MESSAGE_NAME]);
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
        SetUserInSession(user);

        result = controller.LogOut();

        Assert.Throws<KeyNotFoundException>(
            () => sessionMock.GetString(SESSION_USER_STR));
    }
}