using iBugged.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class ProjectsControllerTests
{
    [Test]
    public void ListCallbackReturnsListView()
    {
        ProjectsController projectsController = new ProjectsController();

        var result = projectsController.List();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("List", ((ViewResult)result).ViewName);
    }
}