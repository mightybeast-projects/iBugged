using System.Linq.Expressions;
using iBugged.Controllers;
using iBugged.Models;
using iBugged.Models.Mongo;
using iBugged.ViewModels;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class TicketsControllerTests : ControllerTestsBase<TicketsController>
{
    private readonly TicketViewModel ticketViewModel = TestsData.dummyTicketVM;

    public TicketsControllerTests()
    {
        controller = new TicketsController(
            ticketsRepositoryMock.Object,
            projectsRepositoryMock.Object,
            usersRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [OneTimeSetUp]
    public new void SetUp() => SetUserInSession(user);

    [Test]
    public void List_ReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsView(nameof(controller.List));
    }

    [Test]
    public void List_ReturnsTicketViewModelList()
    {
        result = controller.List();

        ticketsRepositoryMock.Verify(m => m.GetAll());
        projectsRepositoryMock.Verify(m => m.Get(project.id));
        usersRepositoryMock.Verify(m => m.Get(user.id));
        AssertModelIsEqualWithResultModel(
            new List<TicketViewModel> { ticketViewModel });
    }

    [Test]
    public void CreateGet_ReturnsCreateView()
    {
        result = controller.Create();

        AssertViewResultReturnsView(nameof(controller.Create));
    }

    [Test]
    public void CreateGet_SetsUsersAndProjectsLists()
    {
        result = controller.Create();

        AssertUsersAndProjectsListsInViewBag();
    }

    [Test]
    public void CreatePost_RedirectsToListAction()
    {
        result = controller.Create(ticket);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void CreatePost_InsertsTicket()
    {
        result = controller.Create(ticket);

        ticketsRepositoryMock.Verify(m => m.Create(ticket));
    }

    [Test]
    public void CreatePost_UpdatesTicketProject()
    {
        result = controller.Create(ticket);

        projectsRepositoryMock.Verify(m => m.Edit(project.id, project));
    }

    [Test]
    public void EditGet_ReturnsEditView()
    {
        result = controller.Edit(ticket.id);

        AssertViewResultReturnsView(nameof(controller.Edit));
    }

    [Test]
    public void EditGet_ReturnsTicketModel()
    {
        result = controller.Edit(ticket.id);

        AssertUsersAndProjectsListsInViewBag();
        AssertModelIsEqualWithResultModel(ticket);
    }

    [Test]
    public void EditPost_RedirectsToListAction()
    {
        result = controller.Edit(ticket);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void EditPost_EditsTicket()
    {
        result = controller.Edit(ticket);

        ticketsRepositoryMock.Verify(m => m.Edit(ticket.id, ticket));
    }

    [Test]
    public void Delete_RedirectsToListAction()
    {
        result = controller.Delete(ticket.id);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void Delete_DeletesTicket()
    {
        result = controller.Delete(ticket.id);

        ticketsRepositoryMock.Verify(m => m.Delete(ticket.id));
    }

    [Test]
    public void Delete_UpdatesTicketProject()
    {
        result = controller.Delete(ticket.id);

        projectsRepositoryMock.Verify(m => 
            m.Get(It.Is<Expression<Func<Project, bool>>>(e =>
                project.ticketsId.Contains(ticket.id))));
        projectsRepositoryMock.Verify(m => m.Edit(project.id, project));
    }

    private void AssertUsersAndProjectsListsInViewBag()
    {
        AssertViewBagList(controller.ViewBag.projectsList,
            projects.Cast<Document>().ToList());
        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
    }
}