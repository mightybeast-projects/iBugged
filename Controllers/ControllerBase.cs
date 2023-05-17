using iBugged.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iBugged;

[AutoValidateAntiforgeryToken]
public abstract class ControllerBase : Controller
{
    private const string SESSION_USER_STR = "User";

    protected void SetUserInSession(User user) =>
        HttpContext.Session.SetString(SESSION_USER_STR,
            JsonConvert.SerializeObject(user));

    protected User GetUserInSession() =>
        JsonConvert.DeserializeObject<User>(
            HttpContext.Session.GetString(SESSION_USER_STR)!)!;
}