using iBugged.Models;

namespace iBugged.Services;

public interface IUsersService
{
    List<User> Get();
    User Get(string email, string password);
    void Create(User user);
}