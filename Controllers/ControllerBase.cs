using iBugged.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged;

public abstract class ControllerBase : Controller
{
    private const string SESSION_USER_STR = "User";

    protected void SetUserInSession(User user)
    {
        string json = JsonConvert.SerializeObject(user);
        HttpContext.Session.SetString(SESSION_USER_STR, json);
    }

    protected User GetUserInSession()
    {
        string userJson = HttpContext.Session.GetString(SESSION_USER_STR)!;
        return JsonConvert.DeserializeObject<User>(userJson)!;
    }
}