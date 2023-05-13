using iBugged.Models;
using iBugged.Models.Mongo;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

public abstract class ControllerTestsBase<T> : RepositoryMocksSetup
    where T : Controller
{
    protected const string SESSION_USER_STR = "User";
    protected readonly Mock<HttpContext> httpContextMock;
    protected readonly HttpSessionMock sessionMock;
    protected T controller = null!;
    protected IActionResult result = null!;

    public ControllerTestsBase()
    {
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();
    }

    [OneTimeSetUp]
    public new void SetUp() =>
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

    protected void SetUserInSession(User user)
    {
        var json = JsonConvert.SerializeObject(user);
        sessionMock.SetString(SESSION_USER_STR, json);
    }

    protected void AssertViewResultReturnsView(string viewName)
    {
        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual(viewName, ((ViewResult)result).ViewName);
    }

    protected void AssertViewResultReturnsPartialView(string viewName)
    {
        Assert.IsInstanceOf<PartialViewResult>(result);
        Assert.AreEqual(viewName, ((PartialViewResult)result).ViewName);
    }

    protected void AssertRedirectToActionResultReturnsAction(string actionName)
    {
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        Assert.AreEqual(actionName, ((RedirectToActionResult)result).ActionName);
    }

    protected void AssertRedirectResultRedirectsTo(string redirectPath)
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

    protected void AssertModelIsEqualWithResultModel<W>(W model)
    {
        var viewModel = ((ViewResult)result).Model!;
        var modelProject = (W)viewModel;
        AssertObjectsAreEqualAsJsons(model!, viewModel);
    }

    protected void AssertViewBagList(dynamic viewBagList, List<Document> list) =>
        Assert.AreEqual(list[0].id, viewBagList[0].Value);
}