using System.Linq.Expressions;
using iBugged.Controllers;
using iBugged.Models;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class UsersControllerTests : ControllerTestsBase<UsersController>
{
    public UsersControllerTests() =>
        controller = new UsersController(
            usersRepositoryMock.Object,
            projectsRepositoryMock.Object);

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

    [Test]
    public void Delete_RedirectsToListAction()
    {
        result = controller.Delete(user.id);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void Delete_DeletesUser()
    {
        result = controller.Delete(user.id);

        usersRepositoryMock.Verify(m => m.Delete(user.id));
    }

    [Test]
    public void Delete_RemovesUserFromProjectMembers()
    {
        result = controller.Delete(user.id);

        projectsRepositoryMock.Verify(m => m.Edit(project.id, project));
    }
}