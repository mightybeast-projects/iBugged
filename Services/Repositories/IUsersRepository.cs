using iBugged.Models;

namespace iBugged.Services.Repositories;

public interface IUsersRepository : IRepository<User>
{
    User Get(string email, string password);
}