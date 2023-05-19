using iBugged.Services;
using iBugged.Services.Repositories;
using iBugged.Services.ViewServices;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var dbName = "iBugged";
var connectionString = builder.Configuration.GetConnectionString(dbName);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped(typeof(AccessService));
builder.Services.AddScoped(typeof(DashboardService));
builder.Services.AddScoped(typeof(UsersService));
builder.Services.AddScoped(typeof(ProjectsService));
builder.Services.AddScoped(typeof(TicketsService));

builder.Services.AddSingleton<AccessViewService>();
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    new MongoClient(connectionString).GetDatabase(dbName)
);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(20);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Index}");

app.Run();
