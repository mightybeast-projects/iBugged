using iBugged.Services;
using iBugged.Services.Repositories;
using MongoDB.Driver;

var connectionString = "mongodb://localhost:27017";
var dbName = "iBugged_db";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsersRepository, MongoUsersRepository>();
builder.Services.AddScoped<IProjectsRepository, MongoProjectsRepository>();
builder.Services.AddScoped<ITicketsRepository, MongoTicketsRepository>();

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
