using System;
using Microsoft.EntityFrameworkCore;
using CompleteDotNetCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CompleteDotNetCore.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
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
    }
}

