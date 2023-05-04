using iBugged.Controllers;
using iBugged.Models;
using iBugged.ViewModels;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using iBugged.Models.Mongo;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class ProjectsControllerTests : ControllerTestsBase<ProjectsController>
{
    private readonly ProjectViewModel projectVM = TestsData.dummyProjectVM;

    public ProjectsControllerTests()
    {
        controller = new ProjectsController(
            projectsRepositoryMock.Object,
            usersRepositoryMock.Object,
            ticketsRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [OneTimeSetUp]
    public new void SetUp() => SetUserInSession(user);

    [Test]
    public void ListCallbackReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsView(nameof(controller.List));
    }

    [Test]
    public void ListCallbackReturnsCorrectModel()
    {
        result = controller.List();

        projectsRepositoryMock.Verify(m =>
            m.GetAll(It.Is<Expression<Func<Project, bool>>>(e =>
            project.membersId.Contains(user.id))));
        usersRepositoryMock.Verify(m => m.Get(user.id));
        ticketsRepositoryMock.Verify(m => m.Get(ticket.id));
        AssertModelIsEqualWithResultModel(
            new List<ProjectViewModel>{ projectVM });
    }

    [Test]
    public void CreateGetCallbackReturnsCreateView()
    {
        result = controller.Create();

        AssertViewResultReturnsView(nameof(controller.Create));
    }

    [Test]
    public void CreateGetCallbackSetsUsersList()
    {
        result = controller.Create();

        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
    }

    [Test]
    public void CreatePostCallbackRedirectsToListView()
    {
        result = controller.Create(project);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void CreatePostCallbackInsertsNewProject()
    {
        result = controller.Create(project);

        projectsRepositoryMock.Verify(m => m.Create(project));
    }

    [Test]
    public void EditGetCallbackReturnsEditView()
    {
        result = controller.Edit(project.id);

        AssertViewResultReturnsView(nameof(controller.Edit));
    }

    [Test]
    public void EditGetCallbackReturnsCorrectModel()
    {
        result = controller.Edit(project.id);

        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
        AssertModelIsEqualWithResultModel(project);
    }

    [Test]
    public void EditPostCallbackReturnsListView()
    {
        result = controller.Edit(project);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void EditPostCallbackEditsProject()
    {
        result = controller.Edit(project);

        projectsRepositoryMock.Verify(m => m.Edit(project.id, project));
    }

    [Test]
    public void DeleteCallbackReturnsListView()
    {
        result = controller.Delete(project.id);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void DeleteCallbackDeletesProject()
    {
        result = controller.Delete(project.id);

        projectsRepositoryMock.Verify(m => m.Delete(project.id));
        ticketsRepositoryMock.Verify(m => m.Delete(ticket.id));
    }
}