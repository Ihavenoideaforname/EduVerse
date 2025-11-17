using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using EduVerse.Data;
using EduVerse.Models;
using Microsoft.AspNetCore.Identity;
using EduVerse.Services;

var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

string connString = builder.Configuration["ORACLE_CONN_STRING"] ?? throw new InvalidOperationException("Missing connection string!");

builder.Services.AddDbContext<EduVerseContext>(options => options.UseOracle(connString));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication("RememberMeCookie").AddCookie("RememberMeCookie", options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

using(var connection = new OracleConnection(connString))
{
    try
    {
        connection.Open();
        Console.WriteLine("Connection successful!");
    }
    catch(Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}

var app = builder.Build();

await DataSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
