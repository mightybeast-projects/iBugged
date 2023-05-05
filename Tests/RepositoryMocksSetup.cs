using System.Linq.Expressions;
using iBugged.Models;
using iBugged.Services.Repositories;
using Moq;
using NUnit.Framework;

namespace iBugged.Tests;

public class RepositoryMocksSetup
{
    protected readonly Mock<IRepository<User>> usersRepositoryMock;
    protected readonly Mock<IRepository<Ticket>> ticketsRepositoryMock;
    protected readonly Mock<IRepository<Project>> projectsRepositoryMock;
    protected readonly List<User> users = TestsData.users;
    protected readonly List<Project> projects = TestsData.projects;
    protected readonly List<Ticket> tickets = TestsData.tickets;
    protected readonly User user = TestsData.dummyUser;
    protected readonly Project project = TestsData.dummyProject;
    protected readonly Ticket ticket = TestsData.dummyTicket;

    public RepositoryMocksSetup()
    {
        projectsRepositoryMock = new Mock<IRepository<Project>>();
        usersRepositoryMock = new Mock<IRepository<User>>();
        ticketsRepositoryMock = new Mock<IRepository<Ticket>>();
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        SetUpUsersRepositoryMock();
        SetUpTicketsRepositoryMock();
        SetUpProjectsRepositoryMock();
    }

    private void SetUpUsersRepositoryMock()
    {
        usersRepositoryMock.Setup(m => m.GetAll()).Returns(users);
        usersRepositoryMock.Setup(m => m.Get(user.id)).Returns(user);
        users.ForEach(u => usersRepositoryMock
            .Setup(m => m.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns((Expression<Func<User, bool>> predicate) =>
                users.Find(predicate.Compile().Invoke)!));
    }

    private void SetUpTicketsRepositoryMock()
    {
        ticketsRepositoryMock.Setup(m => m.Get(ticket.id)).Returns(ticket);
        ticketsRepositoryMock.Setup(m => m.GetAll()).Returns(tickets);
    }

    private void SetUpProjectsRepositoryMock()
    {
        projectsRepositoryMock.Setup(m => m.GetAll()).Returns(projects);
        projectsRepositoryMock
            .Setup(m => m.GetAll(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                projects.FindAll(predicate.Compile().Invoke));
        projectsRepositoryMock.Setup(m => m.Get(project.id)).Returns(project);
        projectsRepositoryMock
            .Setup(m => m.Get(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                projects.Find(predicate.Compile().Invoke)!);
    }
}