using System;
using System.ComponentModel;
using System.IO;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;


namespace CompleteDotNetCore.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            // Apply migrations, if not applied...
            try
            {
                // If there are migrations that have not been applied...
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception exception)
            {

            }

            // Create Roles, if not created...
            if (!_roleManager.RoleExistsAsync(SD.RoleAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleEmployee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleUserComp)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleUserIndv)).GetAwaiter().GetResult();

                // If Roles not created, create Admin user as well.
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "isitts@hotmail.com",
                    Email = "isitts@hotmail.com",
                    Name = "Scott Isitt",
                    PhoneNumber = "1112223333",
                    Address = "123 Test Street",
                    City = "Citytown",
                    State = "New Staton",
                    ZipCode = "99999"
                }, "Admin@123*").GetAwaiter().GetResult();

                // Assign Admin Role
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(
                    u => u.Email == "isitts@hotmail.com");

                _userManager.AddToRoleAsync(user, SD.RoleAdmin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}

