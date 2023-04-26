using iBugged.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class ProjectsControllerTests
{
    private ProjectsController controller = null!;

    [SetUp]
    public void SetUp()
    {
        controller = new ProjectsController();
    }

    [Test]
    public void ListCallbackReturnsListView()
    {
        var result = controller.List();

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
}