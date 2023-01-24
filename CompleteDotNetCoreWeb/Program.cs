using CompleteDotNetCore.DataAccess;
using CompleteDotNetCore.DataAccess.DbInitializer;
using CompleteDotNetCore.DataAccess.Repository;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container. <-- Dependency injection
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration
.GetConnectionString("AzureConnection")));

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection(
    "StripeSettings:SecretKey").Get<string>();

await SeedDatabaseAsync();

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