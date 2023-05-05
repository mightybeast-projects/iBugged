using iBugged.Controllers;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class UsersControllerTests : ControllerTestsBase<UsersController>
{
    public UsersControllerTests() =>
        controller = new UsersController(usersRepositoryMock.Object);

    [Test]
    public void List_ReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsView(nameof(controller.List));
    }

    [Test]
    public void List_ReturnsUsersListModel()
    {
        result = controller.List();

        AssertModelIsEqualWithResultModel(users);
    }
}