using iBugged.Controllers;
using iBugged.Services;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class UsersControllerTests : ControllerTestsBase<UsersController>
{
    private readonly UsersService usersService;

    public UsersControllerTests()
    {
        usersService = new UsersService(
            usersRepositoryMock.Object,
            projectsRepositoryMock.Object,
            ticketsRepositoryMock.Object);
        controller = new UsersController(usersService);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

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

        AssertModelIsEqualWithViewResultModel(users);
    }

    [Test]
    public void EditGet_ReturnsRegisterView()
    {
        result = controller.Edit(user.id);

        AssertViewResultReturnsView(nameof(controller.Edit));
    }

    [Test]
    public void EditGet_ReturnsUserModel()
    {
        result = controller.Edit(user.id);

        AssertModelIsEqualWithViewResultModel(user);
    }

    [Test]
    public void EditPost_RedirectsToListAction()
    {
        result = controller.Edit(user);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void EditPost_EditsUser()
    {
        result = controller.Edit(user);

        usersRepositoryMock.Verify(m => m.Edit(user.id, user));
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

    [Test]
    public void Delete_RemovesUserFromTicketsAuthorAndAssinee()
    {
        result = controller.Delete(user.id);

        ticketsRepositoryMock.Verify(m => m.Edit(ticket.id, ticket));
    }
}