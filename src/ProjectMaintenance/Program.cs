using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectMaintenance.DAL.Abstract;
using ProjectMaintenance.DAL.Concrete;
using ProjectMaintenance.Data;
using ProjectMaintenance.Models;
using ProjectMaintenance.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AuthenticationConnection") 
    ?? throw new InvalidOperationException("Connection string 'AuthenticationConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var connectionString2 = builder.Configuration.GetConnectionString("ProjectMaintenanceConnection") 
    ?? throw new InvalidOperationException("Connection string 'ProjectMaintenanceConnection' not found.");

builder.Services.AddDbContext<ProjectMaintenanceDbContext>(options =>
    options.UseSqlServer(connectionString2));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMaintenanceTicketRepository, MaintenanceTicketRepository>();
builder.Services.AddScoped<IPMTicketRepository, PMTicketRepository>();

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSingleton<IViewRenderService, ViewRenderService>();

// Added to enable runtime compilation
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddAuthorization(opts => {
    opts.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// By using a scope for the services to be requested below, we limit their lifetime to this set of calls.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Get the IConfiguration service that allows us to query user-secrets and 
        // the configuration on Azure
        var config = app.Services.GetRequiredService<IConfiguration>();

        // Set password with the Secret Manager tool, or store in Azure app configuration
        // dotnet user-secrets set SeedUserPW <pw>
        // -- This method was used earlier, but I am foregoing it since this will only be a temporary
        // -- account that should get deleted early in the life of the program.

        var adminPw = builder.Configuration["AdminAccount:Password"];

        //SeedUsers.Initialize(services, SeedData.UserSeedData, testUserPw).Wait();
        SeedUsers.InitializeAdmin(services, "admin", adminPw, "0").Wait();
        
        var dbContext = services.GetRequiredService<ProjectMaintenanceDbContext>();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
