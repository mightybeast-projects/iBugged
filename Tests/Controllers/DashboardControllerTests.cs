using iBugged.Controllers;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class DashboardControllerTests : ControllerTestsBase<DashboardController>
{
    public DashboardControllerTests() =>
        controller = new DashboardController();

    [Test]
    public void HomeCallbackReturnsHomeView()
    {
        result = controller.Home();

        AssertViewResultReturnsViewWithName("Home");
    }
}