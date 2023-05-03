using iBugged.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

public abstract class ControllerTestsBase<T> where T : Controller
{
    protected const string SESSION_USER_STR = "User";
    protected readonly Mock<HttpContext> httpContextMock;
    protected readonly HttpSessionMock sessionMock;
    protected readonly List<User> users = TestsData.users;
    protected readonly List<Project> projects = TestsData.projects;
    protected readonly List<Ticket> tickets = TestsData.tickets;
    protected readonly User user = TestsData.dummyUser;
    protected readonly Project project = TestsData.dummyProject;
    protected readonly Ticket ticket = TestsData.dummyTicket;
    protected T controller = null!;
    protected IActionResult result = null!;

    public ControllerTestsBase()
    {
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();
    }

    [OneTimeSetUp]
    public void SetUp() =>
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

    protected void SetUserInSession(User user)
    {
        var json = JsonConvert.SerializeObject(user);
        sessionMock.SetString(SESSION_USER_STR, json);
    }

    protected void AssertViewResultReturnsViewWithName(string viewName)
    {
        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual(viewName, ((ViewResult)result).ViewName);
    }

    protected void AssertRedirectToActionResultReturnsActionWithName(string actionName)
    {
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        Assert.AreEqual(actionName, ((RedirectToActionResult)result).ActionName);
    }

    protected void AssertRedirectResultRedirectsToPath(string redirectPath)
    {
        Assert.IsInstanceOf<RedirectResult>(result);
        Assert.AreEqual(redirectPath, ((RedirectResult)result).Url);
    }

    protected void AssertObjectsAreEqualAsJsons(object obj1, object obj2)
    {
        string obj1Json = JsonConvert.SerializeObject(obj1);
        string obj2Json = JsonConvert.SerializeObject(obj2);
        Assert.AreEqual(obj1Json, obj2Json);
    }
}