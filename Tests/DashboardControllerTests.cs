using iBugged.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class DashboardControllerTests : ControllerTestsBase<DashboardController>
{
    [SetUp]
    public void SetUp() => controller = new DashboardController();

    [Test]
    public void HomeCallbackReturnsHomeView()
    {
        result = controller.Home();

        AssertViewResultReturnsViewWithName("Home");
    }
}