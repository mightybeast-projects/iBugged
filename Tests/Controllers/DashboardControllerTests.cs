using iBugged.Controllers;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class DashboardControllerTests : ControllerTestsBase<DashboardController>
{
    public DashboardControllerTests() =>
        controller = new DashboardController();

    [OneTimeSetUp]
    public new void SetUp() => SetUserInSession(user);

    [Test]
    public void Home_ReturnsHomeView()
    {
        result = controller.Home();

        AssertViewResultReturnsView(nameof(controller.Home));
    }
}