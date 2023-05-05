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
    public void List_ReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsView(nameof(controller.List));
    }

    [Test]
    public void List_ReturnsCorrectModel()
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
    public void CreateGet_ReturnsCreateView()
    {
        result = controller.Create();

        AssertViewResultReturnsView(nameof(controller.Create));
    }

    [Test]
    public void CreateGet_SetsUsersList()
    {
        result = controller.Create();

        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
    }

    [Test]
    public void CreatePost_RedirectsToListView()
    {
        result = controller.Create(project);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void CreatePost_InsertsNewProject()
    {
        result = controller.Create(project);

        projectsRepositoryMock.Verify(m => m.Create(project));
    }

    [Test]
    public void EditGet_ReturnsEditView()
    {
        result = controller.Edit(project.id);

        AssertViewResultReturnsView(nameof(controller.Edit));
    }

    [Test]
    public void EditGet_ReturnsCorrectModel()
    {
        result = controller.Edit(project.id);

        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
        AssertModelIsEqualWithResultModel(project);
    }

    [Test]
    public void EditPost_ReturnsListView()
    {
        result = controller.Edit(project);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void EditPost_EditsProject()
    {
        result = controller.Edit(project);

        projectsRepositoryMock.Verify(m => m.Edit(project.id, project));
    }

    [Test]
    public void Delete_RedirectsToListAction()
    {
        result = controller.Delete(project.id);

        AssertRedirectToActionResultReturnsAction(nameof(controller.List));
    }

    [Test]
    public void Delete_DeletesProject()
    {
        result = controller.Delete(project.id);

        projectsRepositoryMock.Verify(m => m.Delete(project.id));
        ticketsRepositoryMock.Verify(m => m.Delete(ticket.id));
    }
}