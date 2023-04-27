using iBugged.Services;
using MongoDB.Driver;

var connectionString = "mongodb://localhost:27017";
var dbName = "iBugged_db";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUsersService, MongoUsersService>();
builder.Services.AddScoped<IProjectsService, MongoProjectsSevice>();

builder.Services.AddSingleton<IMongoDatabase>(sp =>
    new MongoClient(connectionString).GetDatabase(dbName)
);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(10);
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
