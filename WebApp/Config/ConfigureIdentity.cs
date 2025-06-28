using Microsoft.AspNetCore.Identity;
using WebApp.Data;
using WebApp.Identity;

namespace WebApp.Config;

public static partial class Configurator
{
    public static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<AppUser>()
            .AddUserManager<AppUserManager>()
            .AddSignInManager<AppSignInManager>()
            .AddRoles<AppRole>() 
            .AddRoleManager<AppRoleManager>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
       
        return builder;
    }
}