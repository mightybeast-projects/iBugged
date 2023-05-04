using iBugged.Controllers;
using iBugged.Models;
using iBugged.ViewModels;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using iBugged.Models.Mongo;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class ProjectsControllerTests : ControllerTestsBase<ProjectsController>
{
    private readonly Mock<IRepository<User>> usersRepositoryMock;
    private readonly Mock<IRepository<Ticket>> ticketsRepositoryMock;
    private readonly Mock<IRepository<Project>> projectsRepositoryMock;
    private readonly ProjectViewModel projectVM = TestsData.dummyProjectVM;

    public ProjectsControllerTests()
    {
        projectsRepositoryMock = new Mock<IRepository<Project>>();
        usersRepositoryMock = new Mock<IRepository<User>>();
        ticketsRepositoryMock = new Mock<IRepository<Ticket>>();

        controller = new ProjectsController(
            projectsRepositoryMock.Object,
            usersRepositoryMock.Object,
            ticketsRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [OneTimeSetUp]
    public new void SetUp()
    {
        projectsRepositoryMock.Setup(m => m.GetAll()).Returns(projects);
        projectsRepositoryMock.Setup(m => m.Get(project.id)).Returns(project);
        projectsRepositoryMock
            .Setup(m => m.GetAll(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                projects.FindAll(predicate.Compile().Invoke));
        usersRepositoryMock.Setup(m => m.GetAll()).Returns(users);
        usersRepositoryMock.Setup(m => m.Get(user.id)).Returns(user);
        ticketsRepositoryMock.Setup(m => m.Get(ticket.id)).Returns(ticket);

        SetUserInSession(user);
    }

    [Test]
    public void ListCallbackReturnsListView()
    {
        result = controller.List();

        AssertViewResultReturnsViewWithName("List");
    }

    [Test]
    public void ListCallbackReturnListOfProjectViewModels()
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

        AssertViewResultReturnsViewWithName("Create");
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

        AssertRedirectToActionResultReturnsActionWithName("List");
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

        AssertViewResultReturnsViewWithName("Edit");
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

        AssertRedirectToActionResultReturnsActionWithName("List");
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

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void DeleteCallbackDeletesProject()
    {
        result = controller.Delete(project.id);

        projectsRepositoryMock.Verify(m => m.Delete(project.id));
        ticketsRepositoryMock.Verify(m => m.Delete(ticket.id));
    }
}