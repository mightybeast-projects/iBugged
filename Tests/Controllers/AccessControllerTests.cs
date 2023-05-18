using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class AccessControllerTests : ControllerTestsBase<AccessController>
{
    protected readonly AccessService accessService;

    public AccessControllerTests()
    {
        accessService = new AccessService(
            usersRepositoryMock.Object,
            projectsRepositoryMock.Object,
            ticketsRepositoryMock.Object);
        controller = new AccessController(accessService);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public void Index_ReturnsIndexView()
    {
        result = controller.Index();

        AssertViewResultReturnsView(nameof(controller.Index));
    }

    [Test]
    public void RegisterGet_ReturnsRegisterView()
    {
        result = controller.Register();

        AssertViewResultReturnsView(nameof(controller.Register));
    }

    [Test]
    public void RegisterPost_InsertsNewUser()
    {
        result = controller.Register(user);

        usersRepositoryMock.Verify(m => m.Create(user));
    }

    [Test]
    public void RegisterPost_RedirectsToIndexAction()
    {
        result = controller.Register(user);

        AssertRedirectToActionResultReturnsAction(nameof(controller.Index));
    }

    [Test, TestCaseSource(typeof(TestsData), nameof(TestsData.userCases))]
    public void LogIn_RedirectsToDashboard_OnSuccessfulLoginOf(User user)
    {
        result = controller.LogIn(user.email, user.password);

        AssertRedirectResultRedirectsTo("~/Dashboard/Home");
    }

    [Test, TestCaseSource(typeof(TestsData), nameof(TestsData.userCases))]
    public void LogIn_SetsUserInSession_OnSuccessfulLoginOf(User user)
    {
        result = controller.LogIn(user.email, user.password);
        var loggedUserJson = JsonConvert.SerializeObject(user);
        var sessionUserJson = sessionMock.GetString(SESSION_USER_STR);

        Assert.AreEqual(loggedUserJson, sessionUserJson);
    }

    [Test]
    public void LogIn_ReturnsLoginViewWithErrorMessage_OnFailedLogin()
    {
        result = controller.LogIn(string.Empty, string.Empty);

        Assert.IsNotNull(controller.ViewBag.errorMessage);
        AssertViewResultReturnsView(nameof(controller.Index));
    }

    [Test]
    public void LogOut_RedirectsToIndexAction()
    {
        result = controller.LogOut();

        AssertRedirectToActionResultReturnsAction(nameof(controller.Index));
    }

    [Test, TestCaseSource(typeof(TestsData), nameof(TestsData.userCases))]
    public void LogOut_ClearsUserInSession(User user)
    {
        SetUserInSession(user);

        result = controller.LogOut();

        Assert.Throws<KeyNotFoundException>(
            () => sessionMock.GetString(SESSION_USER_STR));
    }
}