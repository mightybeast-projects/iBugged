using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class AccessControllerTests : ControllerTestsBase<AccessController>
{
    private readonly Mock<AccessService> accessServiceMock;
    private readonly User googleUser = TestsData.dummyGoogleUser;

    public AccessControllerTests()
    {
        accessServiceMock = new Mock<AccessService>(
            usersRepositoryMock.Object,
            projectsRepositoryMock.Object,
            ticketsRepositoryMock.Object);
        controller = new AccessController(accessServiceMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [SetUp]
    public new void SetUp()
    {
        accessServiceMock
            .Setup(m => m.GetGoogleUserForSignIn(user.name))
            .ReturnsAsync(user);
        accessServiceMock
            .Setup(m => m.GetGoogleUserForSignIn(googleUser.name))
            .ReturnsAsync(googleUser);
        accessServiceMock
            .Setup(m => m.GetGoogleUserForRegister(user.name))
            .ReturnsAsync(user);
        accessServiceMock
            .Setup(m => m.GetGoogleUserForRegister(googleUser.name))
            .ReturnsAsync(googleUser);
    }

    [Test]
    public void Index_ReturnsIndexView()
    {
        result = controller.Index();

        AssertViewResultReturnsView(nameof(controller.Index));
    }

    [Test]
    public async Task SignInGoogle_ReturnsGoogleUserForSignIn()
    {
        result = await controller.SignInGoogle(user.name);

        accessServiceMock.Verify(m => m.GetGoogleUserForSignIn(user.name));
    }

    [Test]
    public async Task SignInGoogle_RedirectsToDashboard_OnSuccessfulSignIn()
    {
        result = await controller.SignInGoogle(user.name);

        AssertRedirectResultRedirectsTo("~/Dashboard/Home");
    }

    [Test]
    public async Task SignInGoogle_ReturnsIndexView_OnFailedSignIn()
    {
        result = await controller.SignInGoogle(googleUser.name);

        AssertViewResultReturnsView(nameof(controller.Index));
    }

    [Test]
    public void RegisterGet_ReturnsRegisterView()
    {
        result = controller.Register();

        AssertViewResultReturnsView(nameof(controller.Register));
    }

    [Test]
    public async Task RegisterGoogle_ReturnsGoogleUserForRegister()
    {
        result = await controller.RegisterGoogle(user.name);
    
        accessServiceMock.Verify(m => m.GetGoogleUserForRegister(user.name));
    }

    [Test]
    public async Task RegisterGoogle_ReturnsRegisterGoogleView_OnSuccessfulGoogleRegister()
    {
        result = await controller.RegisterGoogle(googleUser.name);

        AssertViewResultReturnsView(nameof(controller.RegisterGoogle));
    }

    [Test]
    public async Task RegisterGoogle_RedirectsToIndex_OnDuplicateRegisterAttempt()
    {
        result = await controller.RegisterGoogle(user.name);

        AssertRedirectToActionResultReturnsAction(nameof(controller.Index));
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
    public void LogIn_ReturnsIndexViewWithErrorMessage_OnFailedLogin()
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