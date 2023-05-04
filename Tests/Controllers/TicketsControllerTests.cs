using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services.Repositories;
using iBugged.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class TicketsControllerTests : ControllerTestsBase<TicketsController>
{
    private readonly Mock<IRepository<Ticket>> ticketsRepositoryMock;
    private readonly Mock<IRepository<Project>> projectsRepositoryMock;
    private readonly Mock<IRepository<User>> usersRepositoryMock;
    private readonly TicketViewModel ticketViewModel = TestsData.dummyTicketVM;

    public TicketsControllerTests()
    {
        ticketsRepositoryMock = new Mock<IRepository<Ticket>>();
        projectsRepositoryMock=  new Mock<IRepository<Project>>();
        usersRepositoryMock = new Mock<IRepository<User>>();
        controller = new TicketsController(
            ticketsRepositoryMock.Object,
            projectsRepositoryMock.Object,
            usersRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [OneTimeSetUp]
    public new void SetUp()
    {
        ticketsRepositoryMock.Setup(m => m.GetAll()).Returns(tickets);
        projectsRepositoryMock.Setup(m => m.GetAll()).Returns(projects);
        projectsRepositoryMock.Setup(m => m.Get(project.id)).Returns(project);
        usersRepositoryMock.Setup(m => m.GetAll()).Returns(users);
        usersRepositoryMock.Setup(m => m.Get(user.id)).Returns(user);

        SetUserInSession(user);
    }

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
        var viewResult = (ViewResult)result;
        var model = (List<TicketViewModel>)viewResult.Model!;
        AssertObjectsAreEqualAsJsons(ticketViewModel, model[0]);
    }

    [Test]
    public void CreateGetCallbackReturnsCreateView()
    {
        result = controller.Create();

        AssertViewResultReturnsViewWithName("Create");
    }

    [Test]
    public void CreateGetCallbackReturnsCorrectModel()
    {
        result = controller.Create();

        var projectsList = controller.ViewBag.projectsList;
        Assert.AreEqual(project.id, projectsList[0].Value);
        Assert.AreEqual(project.name, projectsList[0].Text);
        var usersList = controller.ViewBag.usersList;
        Assert.AreEqual(user.id, usersList[0].Value);
        Assert.AreEqual(user.name, usersList[0].Text);
    }

    [Test]
    public void CreatePostCallbackRedirectsToListView()
    {
        result = controller.Create(ticket);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void CreatePostCallbackInsertsNewTicket()
    {
        result = controller.Create(ticket);

        ticketsRepositoryMock.Verify(m => m.Create(ticket));
    }

    [Test]
    public void DeleteCallbackReturnsListView()
    {
        result = controller.Delete(ticket.id);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void DeleteCallbackDeletesTicket()
    {
        result = controller.Delete(ticket.id);

        ticketsRepositoryMock.Verify(m => m.Delete(ticket.id));
    }
}