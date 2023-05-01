using iBugged.Models;
using NUnit.Framework;

namespace iBugged.Tests;

public static class TestsData
{
    public static User dummyUser = new User()
    {
        name = "MightyBeast",
        email = "mightybeast@labs.com",
        password = "1234567",
        role = Role.ProjectManager
    };

    public static User demoProjectManager = new User()
    {
        name = "Demo Project Manager",
        email = "demoprojectmanager@gmail.com",
        password = "1234567",
        role = Role.ProjectManager
    };

    public static User demoDeveloper = new User()
    {
        name = "Demo Developer",
        email = "demodeveloper@gmail.com",
        password = "1234567",
        role = Role.Developer
    };

    public static User demoTeamMember = new User()
    {
        name = "Demo Team Member",
        email = "demoteammember@gmail.com",
        password = "1234567",
        role = Role.TeamMember
    };

    public static Project dummyProject = new Project()
    {
        name = "Project_1",
        description = "Simple project.",
        membersId = new List<string>() { dummyUser.id }
    };

    public static List<Project> projects = new List<Project>{ dummyProject };

    public static TestCaseData[] userCases =
    {
        new TestCaseData(dummyUser),
        new TestCaseData(demoProjectManager),
        new TestCaseData(demoDeveloper),
        new TestCaseData(demoTeamMember),
    };
}