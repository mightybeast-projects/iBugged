using iBugged.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class DashboardControllerTests
{
    [Test]
    public void HomeCallbackReturnsHomeView()
    {
        DashboardController dashboardController = new DashboardController();

        var result = dashboardController.Home();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("Home", ((ViewResult)result).ViewName);
    }
}