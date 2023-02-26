using System.Net;
using CompleteDotNetCore.DataAccess;
using CompleteDotNetCore.DataAccess.DbInitializer;
using CompleteDotNetCore.DataAccess.Repository;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container. <-- Dependency injection
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration
.GetConnectionString("PostgresConnection")));

builder.Services.Configure<StripeSettingsUtility>(builder.Configuration
    .GetSection("StripeSettings"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// for SeedDatabaseAsync() after app.Run()
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddRazorPages();

// Facebook Login
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
});

// LinkedIn Login
builder.Services.AddAuthentication().AddLinkedIn(options =>
{
    options.ClientId = builder.Configuration["Authentication:LinkedIn:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:LinkedIn:ClientSecret"];
    options.Events = new OAuthEvents()
    {
        OnRemoteFailure = loginFailureHandler =>
        {
            var authProperties = options.StateDataFormat
            .Unprotect(loginFailureHandler.Request.Query["state"]);
            loginFailureHandler.Response.Redirect("/Identity/Account/login");
            loginFailureHandler.HandleResponse(); return Task.FromResult(0);
        }
    };
});

// Redirect to Login Page
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Session Configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();

    // not found error (404)
    if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
    {
        Console.WriteLine(context.Response.StatusCode);
        context.Request.Path = "/Customer/Home/Error";
        await next();
    }
    // // unhandled error (500)
    if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
    {
        Console.WriteLine(context.Response.StatusCode);
        context.Request.Path = "/Customer/Home/Error";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    //CSP prevents cross scripting
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self'; connect-src https://* 'self' wss:; script-src https://* 'self';" +
        " font-src https://* 'self'; style-src https://* 'self' " +
        " 'sha256-47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=' " +
        " 'sha256-9DoVum3m8JKsIY3DTlnlYUaZmF0qX8+iPcNp2w20t90='; " +
        " img-src https://* 'self' http://www.w3.org/2000/svg data:;" +
        " child-src https://* 'self';");
    await next();
});

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection(
    "StripeSettings:SecretKey").Get<string>();

//await SeedDatabaseAsync();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task SeedDatabaseAsync()
{
    using (IServiceScope scope = app.Services.CreateAsyncScope())
    {
        IDbInitializer iDbInitializer = scope.ServiceProvider
            .GetRequiredService<IDbInitializer>();
        await iDbInitializer.InitializeAsync();
    }
    return;
}