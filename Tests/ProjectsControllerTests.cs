using iBugged.Controllers;
using iBugged.Models;
using iBugged.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class ProjectsControllerTests
{
    private Mock<IProjectsService> projectsServiceMock = null!;
    private Mock<HttpContext> httpContextMock = null!;
    private HttpSessionMock sessionMock = null!;
    private ProjectsController controller = null!;
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
    public void ListCallbackReturnsCorrectModelAndListView()
    {
        var result = controller.List();

        projectsServiceMock.Verify(m => m.Get());
        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("List", ((ViewResult)result).ViewName);
    }

    [Test]
    public void CreateGetCallbackReturnsCreateView()
    {
        var result = controller.Create();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("Create", ((ViewResult)result).ViewName);
    }

    [Test]
    public void CreatePostCallbackInsertsProjectAndRedirectsToListView()
    {
        controller!.HttpContext.Session.SetString("Username", "MightyBeast");  

        var result = controller.Create(project);

        projectsServiceMock!.Verify(m => m.Create(project));
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        Assert.AreEqual("List", ((RedirectToActionResult)result).ActionName);
    }

    private Project project = new Project()
    {
        name = "Project_1",
        description = "Simple project.",
        members = new List<string>(){ "MightyBeast" }
    };
}