using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Identity;

namespace WebApp.Config;

public static partial class Configurator
{
    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        string? connStr = builder.Configuration.GetConnectionString("DefaultConnStr");
        if(connStr == null)
        {
            return builder;
        }
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connStr));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        return builder;
    }
}