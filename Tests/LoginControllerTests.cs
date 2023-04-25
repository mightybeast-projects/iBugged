using iBugged.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests;

[TestFixture]
public class LoginControllerTests
{
    private Mock<IUsersDataService>? mock;
    private LoginController? loginController;

    [SetUp]
    public void SetUp()
    {
        mock = new Mock<IUsersDataService>();
        loginController = new LoginController(mock.Object);
    }

    [Test]
    public void IndexCallbackReturnsIndexView()
    {
        var result = loginController!.Index();

        Assert.IsInstanceOf<ViewResult>(result);
        Assert.AreEqual("Index", ((ViewResult)result).ViewName);
    }
}