using iBugged.Controllers;
using iBugged.Models;
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
    private readonly Project project = TestsData.dummyProject;
    private readonly User user = TestsData.dummyUser;

    [SetUp]
    public void SetUp()
    {
        projectsRepositoryMock = new Mock<IProjectsRepository>();
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();

        projectsRepositoryMock.Setup(m => m.Get()).Returns(TestsData.projects);
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

        controller = new ProjectsController(projectsRepositoryMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public void ListCallbackReturnsListViewWithCorrectModel()
    {
        result = controller.List();

        projectsRepositoryMock.Verify(m => m.Get());
        var viewResult = (ViewResult)result;
        var model = (List<Project>)viewResult.Model!;
        Assert.AreEqual(project, model[0]);
        AssertViewResultReturnsViewWithName("List");
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