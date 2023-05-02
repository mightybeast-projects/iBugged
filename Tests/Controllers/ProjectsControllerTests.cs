using iBugged.Controllers;
using iBugged.Models;
using iBugged.ViewModels;
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
    private readonly List<User> users = TestsData.users;
    private readonly List<Project> projects = TestsData.projects;
    private readonly Project project = TestsData.dummyProject;
    private readonly User user = TestsData.dummyUser;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        projectsRepositoryMock = new Mock<IProjectsRepository>();
        usersRepositoryMock = new Mock<IUsersRepository>();

        projectsRepositoryMock.Setup(m => m.Get()).Returns(projects);
        usersRepositoryMock.Setup(m => m.Get()).Returns(users);
        usersRepositoryMock.Setup(m => m.Get(user.id)).Returns(user);

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
        ProjectViewModel projectVM = new ProjectViewModel()
        {
            project = project,
            members = new List<User>{ user }
        };

        result = controller.List();

        projectsRepositoryMock.Verify(m => m.Get());
        usersRepositoryMock.Verify(m => m.Get(user.id));
        var viewResult = (ViewResult)result;
        var model = (List<ProjectViewModel>)viewResult.Model!;
        var projectViewModelJson = JsonConvert.SerializeObject(projectVM);
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
    public void CreateGetCallbackReturnsCorrectModel()
    {
        result = controller.Create();

        var model = ((ViewResult)result).Model;
        Assert.IsInstanceOf<ProjectCreationViewModel>(model);
        Assert.AreEqual(user.id,
            ((ProjectCreationViewModel)model!).users[0].Value);
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