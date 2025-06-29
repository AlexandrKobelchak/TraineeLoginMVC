public static partial class Program{
    public static WebApplicationBuilder ConfigureMVC(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        return builder;
    }
}