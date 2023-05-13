using System.Linq.Expressions;
using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using iBugged.ViewModels;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class DashboardControllerTests : ControllerTestsBase<DashboardController>
{
    private readonly DashboardService dashboardService;
    private readonly TicketViewModel ticketViewModel = TestsData.dummyTicketVM;

    public DashboardControllerTests()
    {
        dashboardService = new DashboardService(
            usersRepositoryMock.Object,
            projectsRepositoryMock.Object,
            ticketsRepositoryMock.Object);
        controller = new DashboardController(dashboardService);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [OneTimeSetUp]
    public new void SetUp() => SetUserInSession(user);

    [Test]
    public void Home_ReturnsHomeView()
    {
        result = controller.Home();

        AssertViewResultReturnsView(nameof(controller.Home));
    }

    [Test]
    public void Home_ReturnsTicketViewModelsList()
    {
        result = controller.Home();

        ticketsRepositoryMock.Verify(m =>
            m.GetAll(It.IsAny<Expression<Func<Ticket, bool>>>()));
        projectsRepositoryMock.Verify(m => m.Get(project.id));
        usersRepositoryMock.Verify(m => m.Get(user.id));
        AssertModelIsEqualWithViewResultModel(
            new List<TicketViewModel> { ticketViewModel });
    }

    [Test]
    public void Close_ReturnsHomeView()
    {
        result = controller.CloseTicket(ticket.id);

        AssertRedirectToActionResultReturnsAction(nameof(controller.Home));
    }

    [Test]
    public void Close_ClosesTicket()
    {
        result = controller.CloseTicket(ticket.id);

        ticketsRepositoryMock.Verify(m => m.Get(ticket.id));
        ticketsRepositoryMock.Verify(m => m.Edit(ticket.id, ticket));
    }
}