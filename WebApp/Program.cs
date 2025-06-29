namespace WebApp;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.ConfigureDatabase();
        builder.ConfigureIdentity();
        builder.ConfigureAuthentication();
        builder.ConfigureAuthorization();
        builder.ConfigureMVC();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        { 
            app.UseMigrationsEndPoint();
        }
        else
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