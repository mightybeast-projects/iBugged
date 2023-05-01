using iBugged.Models;

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

    public static Project dummyProject = new Project()
    {
        name = "Project_1",
        description = "Simple project.",
        membersId = new List<string>() { dummyUser.id }
    };

    public static List<Project> projects = new List<Project>{ dummyProject };
}