using iBugged.Models;
using Newtonsoft.Json;

namespace iBugged.Services.ViewServices;

public class AccessViewService
{
    private readonly IHttpContextAccessor accessor;
    public User user => GetUser();

    public AccessViewService(IHttpContextAccessor accessor) =>
        this.accessor = accessor;
    
    public User GetUser()
    {
        ISession session = accessor.HttpContext!.Session;
        string userJson = session.GetString("User")!;
        return JsonConvert.DeserializeObject<User>(userJson)!;
    }
}