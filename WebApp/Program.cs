using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Identity;
using WebApp.Identity.ViewModels;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        string? connStr = builder.Configuration.GetConnectionString("DefaultConnStr");
        if(connStr == null)
        {
            return;
        }
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connStr));

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.LoginPath = "/Account/Login";
            });

        builder.Services.AddIdentityCore<AppUser>()
            .AddUserManager<AppUserManager>()
            .AddSignInManager<AppSignInManager>()
            .AddRoles<AppRole>() 
            .AddRoleManager<AppRoleManager>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

        });



        builder.Services.AddAuthorization(options => {
            options.AddPolicy("RequireModerator",
                policy => policy.RequireRole("Administrator", "Moderator").RequireAuthenticatedUser());

            options.AddPolicy("RequireAdministrator",
                policy => policy.RequireRole("Administrator").RequireAuthenticatedUser());
        });

        builder.Services.AddAuthorization(options => {
            options.AddPolicy("RequireModerator",
                policy => policy.RequireRole("Administrator", "Moderator").RequireAuthenticatedUser());

            options.AddPolicy("RequireAdministrator",
                policy => policy.RequireRole("Administrator").RequireAuthenticatedUser());
        });
        
        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
       

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}