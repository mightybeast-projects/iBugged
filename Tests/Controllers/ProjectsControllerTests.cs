using iBugged.Controllers;
using iBugged.Models;
using iBugged.Models.ViewModels;
using iBugged.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class ProjectsControllerTests : ControllerTestsBase<ProjectsController>
{
    private Mock<IProjectsRepository> projectsRepositoryMock = null!;
    private Mock<IUsersRepository> usersRepositoryMock = null!;
    private readonly List<Project> projects = TestsData.projects;
    private readonly Project project = TestsData.dummyProject;
    private readonly User user = TestsData.dummyUser;

    [SetUp]
    public void SetUp()
    {
        projectsRepositoryMock = new Mock<IProjectsRepository>();
        usersRepositoryMock = new Mock<IUsersRepository>();
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();

        projectsRepositoryMock.Setup(m => m.Get()).Returns(projects);
        usersRepositoryMock.Setup(m => m.Get(user.id)).Returns(user);
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

        controller = new ProjectsController(
            projectsRepositoryMock.Object,
            usersRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
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
        ProjectViewModel projectViewModel = new ProjectViewModel()
        {
            project = project,
            members = new List<User>{ user }
        };

        result = controller.List();

        projectsRepositoryMock.Verify(m => m.Get());
        usersRepositoryMock.Verify(m => m.Get(user.id));
        var viewResult = (ViewResult)result;
        var model = (List<ProjectViewModel>)viewResult.Model!;
        var projectViewModelJson = JsonConvert.SerializeObject(projectViewModel);
        var resultProjectViewModelJson = JsonConvert.SerializeObject(model[0]);
        Assert.AreEqual(projectViewModelJson, resultProjectViewModelJson);
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
        sessionMock.SetString("User", JsonConvert.SerializeObject(user));

        result = controller.Create(project);

        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    [Test]
    public void CreatePostCallbackInsertsNewProject()
    {
        sessionMock.SetString("User", JsonConvert.SerializeObject(user));

        result = controller.Create(project);

        projectsRepositoryMock.Verify(m => m.Create(project));
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