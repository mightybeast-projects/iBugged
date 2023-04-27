using iBugged.Controllers;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

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