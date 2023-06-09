using iBugged.Models;
using iBugged.Models.Enums;
using iBugged.ViewModels;
using NUnit.Framework;

namespace iBugged.Tests;

public static class TestsData
{
    public static User dummyUser => new User()
    {
        id = "1",
        name = "MightyBeast",
        email = "mightybeast@labs.com",
        password = "1234567",
        role = Role.ProjectManager
    };

    public static User demoProjectManager => new User()
    {
        id = "2",
        name = "Demo Project Manager",
        email = "demoprojectmanager@gmail.com",
        password = "1234567",
        role = Role.ProjectManager
    };

    public static User demoDeveloper => new User()
    {
        id = "3",
        name = "Demo Developer",
        email = "demodeveloper@gmail.com",
        password = "1234567",
        role = Role.Developer
    };

    public static User demoTeamMember => new User()
    {
        id = "4",
        name = "Demo Team Member",
        email = "demoteammember@gmail.com",
        password = "1234567",
        role = Role.TeamMember
    };
    
    public static User dummyGoogleUser => new User()
    {
        id = "5",
        name = "GoogleUserMightyBeast",
        email = "zgpoter@gmail.com",
        password = "1234567",
        role = Role.ProjectManager
    };

    public static Project dummyProject => new Project()
    {
        id = "1",
        name = "Project_1",
        description = "Simple project.",
        membersId = new List<string>{ dummyUser.id },
        ticketsId = new List<string>{ "1" }
    };

    public static Ticket dummyTicket => new Ticket()
    {
        id = "1",
        title = "Ticket 1",
        description = "Just a simple ticket.",
        priority = Priority.Low,
        ticketType = TicketType.Bug,
        projectId = "1",
        assigneeId = dummyUser.id,
        authorId = dummyUser.id,
        creationDate = DateTime.Today
    };

    public static TestCaseData[] userCases => new TestCaseData[]
    {
        new TestCaseData(dummyUser),
        new TestCaseData(demoProjectManager),
        new TestCaseData(demoDeveloper),
        new TestCaseData(demoTeamMember),
    };

    public static List<User> users => new List<User>
    {
        dummyUser,
        demoProjectManager,
        demoDeveloper,
        demoTeamMember
    };

    public static List<Ticket> tickets => new List<Ticket>{ dummyTicket };

    public static List<Project> projects => new List<Project>{ dummyProject };

    public static TicketViewModel dummyTicketVM => new TicketViewModel()
    {
        ticket = dummyTicket,
        project = dummyProject,
        assignee = dummyUser,
        author = dummyUser
    };

    public static ProjectViewModel dummyProjectVM => new ProjectViewModel()
    {
        project = dummyProject,
        members = new List<User>{ dummyUser },
        tickets = new List<Ticket> { dummyTicket }
    };
}