using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class TicketsControllerTests : ControllerTestsBase<TicketsController>
{
    private readonly Mock<IRepository<Ticket>> ticketsRepository;
    private readonly Mock<IRepository<Project>> projectsRepository;

    public TicketsControllerTests()
    {
        ticketsRepository = new Mock<IRepository<Ticket>>();
        projectsRepository=  new Mock<IRepository<Project>>();
        controller = new TicketsController(
            ticketsRepository.Object,
            projectsRepository.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [OneTimeSetUp]
    public new void SetUp()
    {
        ticketsRepository.Setup(m => m.GetAll()).Returns(tickets);
        projectsRepository.Setup(m => m.GetAll()).Returns(projects);

        SetUserInSession(user);
    }

    [Test]
    public void ListCallbackReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsViewWithName("List");
    }

    [Test]
    public void ListCallbackReturnsCorrectModel()
    {
        result = controller.List();

        var model = ((ViewResult)result).Model;
        Assert.IsInstanceOf<List<Ticket>>(model);
        Assert.AreEqual(ticket, ((List<Ticket>)model!)[0]);
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

        ticketsRepository.Verify(m => m.Create(ticket));
    }
}