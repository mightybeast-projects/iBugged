using iBugged.Models;
using Newtonsoft.Json;

namespace iBugged.Services;

public class AccessViewService
{
    private readonly IHttpContextAccessor accessor;
    public User user => GetUser();
    private User _user = null!;

    public AccessViewService(IHttpContextAccessor accessor) =>
        this.accessor = accessor;
    
    public User GetUser()
    {
        ISession session = accessor.HttpContext!.Session;
        string userJson = session.GetString("User")!;
        if (userJson == null)
            _user = null!;
        if (_user == null && userJson != null)
            _user = JsonConvert.DeserializeObject<User>(userJson)!;
        return _user!;
    }
}