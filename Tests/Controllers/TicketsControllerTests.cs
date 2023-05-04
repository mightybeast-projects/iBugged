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
    public void ListCallbackReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsViewWithName("List");
    }

    [Test]
    public void ListCallbackReturnsListOfTicketViewModels()
    {
        result = controller.List();

        ticketsRepositoryMock.Verify(m => m.GetAll());
        projectsRepositoryMock.Verify(m => m.Get(project.id));
        usersRepositoryMock.Verify(m => m.Get(user.id));
        AssertModelIsEqualWithResultModel(
            new List<TicketViewModel> { ticketViewModel });
    }

    [Test]
    public void CreateGetCallbackReturnsCreateView()
    {
        result = controller.Create();

        AssertViewResultReturnsViewWithName("Create");
    }

    [Test]
    public void CreateGetCallbackSetsUsersAndProjectsLists()
    {
        result = controller.Create();

        AssertViewBagList(controller.ViewBag.projectsList,
            projects.Cast<Document>().ToList());
        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
    }

    [Test]
    public void CreatePostCallbackRedirectsToListView()
    {
        result = controller.Create(ticket);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void CreatePostCallbackInsertsNewTicketAndUpdatesProject()
    {
        result = controller.Create(ticket);

        ticketsRepositoryMock.Verify(m => m.Create(ticket));
        projectsRepositoryMock.Verify(m => m.Edit(project.id, project));
    }

    [Test]
    public void EditGetCallbackReturnsEditView()
    {
        result = controller.Edit(ticket.id);

        AssertViewResultReturnsViewWithName("Edit");
    }

    [Test]
    public void EditGetCallbackSetsUsersAndProjectsLists()
    {
        result = controller.Edit(ticket.id);

        AssertViewBagList(controller.ViewBag.projectsList,
            projects.Cast<Document>().ToList());
        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
        AssertModelIsEqualWithResultModel(ticket);
    }

    [Test]
    public void EditPostCallbackReturnsListView()
    {
        result = controller.Edit(ticket);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void EditPostCallbackEditsTicket()
    {
        result = controller.Edit(ticket);

        ticketsRepositoryMock.Verify(m => m.Edit(ticket.id, ticket));
    }

    [Test]
    public void DeleteCallbackReturnsListView()
    {
        result = controller.Delete(ticket.id);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void DeleteCallbackDeletesTicketAndUpdatesProject()
    {
        result = controller.Delete(ticket.id);

        ticketsRepositoryMock.Verify(m => m.Delete(ticket.id));
        projectsRepositoryMock
            .Verify(m =>
            m.Get(It.Is<Expression<Func<Project, bool>>>(e =>
            project.ticketsId.Contains(ticket.id))));
        projectsRepositoryMock.Verify(m => m.Edit(project.id, project));
    }
}