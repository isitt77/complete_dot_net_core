using System;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace CompleteDotNetCoreWeb.ServiceExtensions
{
    public static class ExternalLoginServices
    {
        public static void AddExternalLogin(
            this IServiceCollection services, IConfiguration config)
        {
            // Facebook Login
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = config["Authentication:Facebook:AppId"];
                options.AppSecret = config["Authentication:Facebook:AppSecret"];
            });

            // LinkedIn Login
            services.AddAuthentication().AddLinkedIn(options =>
            {
                options.ClientId = config["Authentication:LinkedIn:ClientId"];
                options.ClientSecret = config["Authentication:LinkedIn:ClientSecret"];
                options.Events = new OAuthEvents()
                {
                    OnRemoteFailure = loginFailureHandler =>
                    {
                        var authProperties = options.StateDataFormat
                        .Unprotect(loginFailureHandler.Request
                        .Query["state"]);
                        loginFailureHandler.Response.Redirect(
                            "/Identity/Account/login");
                        loginFailureHandler.HandleResponse();
                        return Task.FromResult(0);
                    }
                };
            });

            // Google Login
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = config["Authentication:Google:ClientId"];
                options.ClientSecret = config["Authentication:Google:ClientSecret"];
            });
        }
    }
}

