using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public abstract class ControllerTestsBase<T> where T : Controller
{
    protected Mock<HttpContext> httpContextMock = null!;
    protected HttpSessionMock sessionMock = null!;
    protected T controller = null!;
    protected IActionResult result = null!;

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
}