using BenchmarkDotNet.Running;
using CarRentalWebsite.Controllers;
using CarRentalWebsite.Data;
using CarRentalWebsite.Database;
using CarRentalWebsite.Hubs;
using CarRentalWebsite.Services;
using CarRentalWebsite.SmtpService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<DBContext>(
              options => options.UseSqlServer(builder.Configuration.GetConnectionString("CRWDbConnection")));
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddHttpReportsDashboard().AddSQLServerStorage();
builder.Services.AddHttpReports().AddHttpTransport();


builder.Services.AddSingleton<Smtp, Smtp>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseExceptionHandler("/Error");

//Logging middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        string folder = @"C:\Temp\";
        string fileName = "LoggingExceptionsCarRentalWebsite.txt";
        string fullPath = folder + fileName;
        File.WriteAllBytes(fullPath, Encoding.Default.GetBytes(ex.ToString()));

        ex.Data.Add("Request", $"{context.Request.Method} {context.Request.Path}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error occurred, please try again later.");
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/ChatHub");
app.MapRazorPages();

var summary = BenchmarkRunner.Run<AccountController>();
Console.WriteLine(summary);

//app.UseHttpReportsDashboard();//Use HttpReports Dashboard
app.UseHttpReports(); //Use HttpReports

app.Run();
