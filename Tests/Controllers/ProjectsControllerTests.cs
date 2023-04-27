using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests.Controllers;

[TestFixture]
public class ProjectsControllerTests : ControllerTestsBase<ProjectsController>
{
    private Mock<IProjectsService> projectsServiceMock = null!;
    private readonly Project project = TestsData.dummyProject;

    [SetUp]
    public void SetUp()
    {
        projectsServiceMock = new Mock<IProjectsService>();
        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();

        projectsServiceMock.Setup(m => m.Get()).Returns(TestsData.projects);
        httpContextMock.Setup(s => s.Session).Returns(sessionMock);

        controller = new ProjectsController(projectsServiceMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public void ListCallbackReturnsListViewWithCorrectModel()
    {
        result = controller.List();

        projectsServiceMock.Verify(m => m.Get());
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
    public void CreatePostCallbackInsertsProjectAndRedirectsToListView()
    {
        sessionMock.SetString("Username", project.members[0]);

        result = controller.Create(project);

        projectsServiceMock.Verify(m => m.Create(project));
        AssertRedirectToActionResultReturnsActionWithName("List");
    }
}