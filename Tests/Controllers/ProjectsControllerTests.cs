using iBugged.Controllers;
using iBugged.Models;
using iBugged.ViewModels;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class ProjectsControllerTests : ControllerTestsBase<ProjectsController>
{
    private readonly Mock<IProjectsRepository> projectsRepositoryMock;
    private readonly Mock<IUsersRepository> usersRepositoryMock;
    private readonly ProjectViewModel projectVM = TestsData.dummyProjectVM;
    private readonly List<User> users = TestsData.users;
    private readonly List<Project> projects = TestsData.projects;
    private readonly Project project = TestsData.dummyProject;
    private readonly Project editedProject = TestsData.dummyProject;
    private readonly User user = TestsData.dummyUser;

    public ProjectsControllerTests()
    {
        projectsRepositoryMock = new Mock<IProjectsRepository>();
        usersRepositoryMock = new Mock<IUsersRepository>();

        controller = new ProjectsController(
            projectsRepositoryMock.Object,
            usersRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [OneTimeSetUp]
    public new void SetUp()
    {
        projectsRepositoryMock.Setup(m => m.Get()).Returns(projects);
        projectsRepositoryMock.Setup(m => m.Get(project.id)).Returns(project);
        usersRepositoryMock.Setup(m => m.Get()).Returns(users);
        usersRepositoryMock.Setup(m => m.Get(user.id)).Returns(user);
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

        projectsRepositoryMock.Verify(m => m.Get());
        usersRepositoryMock.Verify(m => m.Get(user.id));
        var viewResult = (ViewResult)result;
        var model = (List<ProjectViewModel>)viewResult.Model!;
        AssertObjectsAreEqualAsJsons(projectVM, model[0]);
    }

    [Test]
    public void CreateGetCallbackReturnsCreateView()
    {
        result = controller.Create();

        AssertViewResultReturnsViewWithName("Create");
    }

    [Test]
    public void CreateGetCallbackSetsUsersListInViewBag()
    {
        result = controller.Create();

        Assert.AreEqual(users[0].name, controller.ViewBag.usersList[0].Text);
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
    public void EditGetCallbackReturnsProjectModel()
    {
        result = controller.Edit(project.id);

        var model = ((ViewResult)result).Model!;
        var modelProject = (Project) model;
        Assert.AreEqual(project, modelProject);
        Assert.AreEqual(users[0].name, controller.ViewBag.usersList[0].Text);
    }

    [Test]
    public void EditPostCallbackReturnsListView()
    {
        result = controller.Edit(editedProject);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void EditPostCallbackEditsProject()
    {
        editedProject.name = "EditedProject";

        result = controller.Edit(editedProject);

        projectsRepositoryMock
            .Verify(m => m.Edit(editedProject.id, editedProject));
    }

    [Test]
    public void DeleteCallbackDeletesProject()
    {
        result = controller.Delete(project.id);

        projectsRepositoryMock.Verify(m => m.Delete(project.id));
    }

    [Test]
    public void DeleteCallbackReturnsListView()
    {
        result = controller.Delete(project.id);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }
}