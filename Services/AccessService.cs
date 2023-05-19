using GoogleAuthentication.Services;
using iBugged.Models;
using iBugged.Services.Repositories;
using Newtonsoft.Json;

namespace iBugged.Services;

public class AccessService : Service
{
    private const string CLIENT_ID = "493795471117-io9gpva1nr1mguegl9queu6s5571od4e.apps.googleusercontent.com";
    private const string CLIENT_SECRET = "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";
    private const string GOOGLE_SIGN_IN_URL = "https://ibugged.azurewebsites.net/Access/SignInGoogle";
    private const string GOOGLE_REGISTER_URL = "https://ibugged.azurewebsites.net/Access/RegisterGoogle";

    public AccessService(
        IRepository<User> usersRepository,
        IRepository<Project> projectsRepository,
        IRepository<Ticket> ticketsRepository)
        : base(usersRepository, projectsRepository, ticketsRepository) { }

    public User Get(string email, string password) =>
        usersRepository.Get(f => f.email == email && f.password == password);

    public void Create(User user) => usersRepository.Create(user);

    public string GetGoogleSignInUrl() =>
        GoogleAuth.GetAuthUrl(CLIENT_ID, GOOGLE_SIGN_IN_URL);
    
    public string GetGoogleRegisterUrl() =>
        GoogleAuth.GetAuthUrl(CLIENT_ID, GOOGLE_REGISTER_URL);

    public virtual async Task<User> GetGoogleUserForSignIn(string code) =>
        await GetGoogleUser(code, GOOGLE_SIGN_IN_URL);

    public virtual async Task<User> GetGoogleUserForRegister(string code) =>
        await GetGoogleUser(code, GOOGLE_REGISTER_URL);

    public async Task<User> GetGoogleUser(string code, string url)
    {
        var token = await GoogleAuth
            .GetAuthAccessToken(code, CLIENT_ID, CLIENT_SECRET, url);
        var userProfile = await GoogleAuth
            .GetProfileResponseAsync(token.AccessToken);
        User user = JsonConvert.DeserializeObject<User>(userProfile)!;
        user.password = user.id;
        return user;
    }
}