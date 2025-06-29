using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

public static partial class Program{
    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Identity.Application";
                options.DefaultSignInScheme = "Identity.Application";
                options.DefaultChallengeScheme = "Identity.Application";
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            })
            .AddIdentityCookies();
         
        return builder;
    }

}