using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services.Repositories;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class TicketsControllerTests : ControllerTestsBase<TicketsController>
{
    private Mock<ITicketsRepository> ticketsRepository = null!;
    private readonly Ticket ticket = TestsData.dummyTicket;

    [SetUp]
    public override void SetUp()
    {
        ticketsRepository = new Mock<ITicketsRepository>();

        controller = new TicketsController(ticketsRepository.Object);
    }

    [Test]
    public void ListCallbackReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsViewWithName("List");
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