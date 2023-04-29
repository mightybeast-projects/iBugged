using iBugged.Models;

namespace iBugged.Services;

public interface IUsersService : IService<User>
{
    User Get(string email, string password);
}