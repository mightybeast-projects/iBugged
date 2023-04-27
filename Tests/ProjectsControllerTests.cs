using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class ProjectsControllerTests : ControllerTestsBase<ProjectsController>
{
    private Mock<IProjectsService> projectsServiceMock = null!;
    private List<Project> projects = new List<Project>();

    [SetUp]
    public void SetUp()
    {
        projects.Add(project);

        httpContextMock = new Mock<HttpContext>();
        sessionMock = new HttpSessionMock();
        projectsServiceMock = new Mock<IProjectsService>();

        httpContextMock.Setup(s => s.Session).Returns(sessionMock);
        projectsServiceMock.Setup(m => m.Get()).Returns(projects);

        controller = new ProjectsController(projectsServiceMock.Object);
        controller.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public void ListCallbackReturnsListViewWithCorrectModel()
    {
        result = controller.List();

        projectsServiceMock.Verify(m => m.Get());
        AssertViewResultReturnsViewWithName("List");
        var viewResult = (ViewResult)result;
        var model = (List<Project>)viewResult.Model!;
        Assert.AreEqual(project, model[0]);
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
        sessionMock.SetString("Username", project.members![0]);

        result = controller.Create(project);

        projectsServiceMock.Verify(m => m.Create(project));
        AssertRedirectToActionResultReturnsActionWithName("List");
    }

    private Project project = new Project()
    {
        name = "Project_1",
        description = "Simple project.",
        members = new List<string>() { "MightyBeast" }
    };
}