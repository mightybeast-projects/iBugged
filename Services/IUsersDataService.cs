using iBugged.Models;

public interface IUsersDataService
{
    List<User> Get();
    User Get(string email, string password);
    void Create(User user);
}