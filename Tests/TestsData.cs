using iBugged.Models;

namespace iBugged.Tests;

public static class TestsData
{
    public static User dummyUser = new User()
    {
        name = "MightyBeast",
        email = "mightybeast@labs.com",
        password = "1234567",
        organization = "MightyBeastLabs",
        role = Role.ProjectManager
    };

    public static Project dummyProject = new Project()
    {
        name = "Project_1",
        description = "Simple project.",
        members = new List<string>() { "MightyBeast" }
    };

    public static List<Project> projects = new List<Project>{ dummyProject };
}