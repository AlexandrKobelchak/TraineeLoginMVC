using Microsoft.EntityFrameworkCore;
using WebApp.Data;

public static partial class Program
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