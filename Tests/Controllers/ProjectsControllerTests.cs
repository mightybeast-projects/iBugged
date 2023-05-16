using iBugged.Controllers;
using iBugged.Models;
using iBugged.ViewModels;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using iBugged.Models.Mongo;
using iBugged.Services;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class ProjectsControllerTests : ControllerTestsBase<ProjectsController>
{
    private readonly ProjectsService projectsService;
    private readonly ProjectViewModel projectVM = TestsData.dummyProjectVM;

    public ProjectsControllerTests()
    {
        projectsService = new ProjectsService(
            usersRepositoryMock.Object,
            projectsRepositoryMock.Object,
            ticketsRepositoryMock.Object);
        controller = new ProjectsController(projectsService);
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
    public void List_WithSearch_ReturnsFilteredViewModelList()
    {
        string searchString = "Project_";

        result = controller.List(searchString);

        projectsRepositoryMock.Verify(m =>
            m.GetAll(It.IsAny<Expression<Func<Project, bool>>>()));
        usersRepositoryMock.Verify(m => m.Get(user.id));
        ticketsRepositoryMock.Verify(m => m.Get(ticket.id));
        AssertModelIsEqualWithViewResultModel(
            new List<ProjectViewModel> { projectVM }
        );
    }

    [Test]
    public void List_ReturnsProjectViewModelList()
    {
        result = controller.List();

        projectsRepositoryMock.Verify(m => m.GetAll());
        usersRepositoryMock.Verify(m => m.Get(user.id));
        ticketsRepositoryMock.Verify(m => m.Get(ticket.id));
        AssertModelIsEqualWithViewResultModel(
            new List<ProjectViewModel>{ projectVM }
        );
    }

    [Test]
    public void CreateGet_ReturnsCreateView()
    {
        result = controller.Create();

        AssertViewResultReturnsPartialView(nameof(controller.Create));
    }

    [Test]
    public void CreateGet_SetsUsersList()
    {
        result = controller.Create();

        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
    }

    [Test]
    public void CreatePost_RedirectsToListAction()
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

        AssertViewResultReturnsPartialView(nameof(controller.Edit));
    }

    [Test]
    public void EditGet_ReturnsProjectModel()
    {
        result = controller.Edit(project.id);

        AssertViewBagList(controller.ViewBag.usersList,
            users.Cast<Document>().ToList());
        AssertModelIsEqualWithPartialViewResultModel(project);
    }

    [Test]
    public void EditPost_RedirectsToListAction()
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