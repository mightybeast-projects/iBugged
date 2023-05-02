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
    private Mock<ITicketsRepository> ticketsRepository = null!;
    private readonly List<Ticket> tickets = TestsData.tickets;
    private readonly Ticket ticket = TestsData.dummyTicket;

    [SetUp]
    public override void SetUp()
    {
        ticketsRepository = new Mock<ITicketsRepository>();

        ticketsRepository.Setup(m => m.Get()).Returns(tickets);

        controller = new TicketsController(ticketsRepository.Object);
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