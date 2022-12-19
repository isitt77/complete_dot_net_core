using System;
using Microsoft.EntityFrameworkCore;
using CompleteDotNetCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CompleteDotNetCore.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Creates the Category table
        public DbSet<Category> Categories { get; set; }

        // Creates the CoverType table
        public DbSet<CoverType> CoverTypes { get; set; }

        // Creates the Product table
        public DbSet<Product> Products { get; set; }

        // Creates the ApplicationUser table
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Creates the Company table
        public DbSet<Company> Companies { get; set; }

        // Creates the ShoppingCart table
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        // Creates OrderHeader table
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        // Creates Order Detail table
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

