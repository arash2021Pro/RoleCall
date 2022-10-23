using Microsoft.AspNetCore.Authentication.Cookies;

namespace LicenseProject.StartupModuleServices.AuthenticationService;

public static class AuthService
{
    public static void RunAuthentication(this IServiceCollection service)
    {
        service.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(option =>
        {
            //      option.Cookie.Expiration = new TimeSpan().Add(TimeSpan.FromMinutes(15));
            option.Cookie.Name = "SP3GGS-ID2-cookie";
            option.LoginPath = "/Admin/Login";
            option.LogoutPath = "/Admin/Logout";
            option.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            option.SlidingExpiration = true;
            option.Cookie.HttpOnly = true;
            option.Cookie.IsEssential = true;
            option.Cookie.SameSite = SameSiteMode.Strict;
            
        });
    }
}