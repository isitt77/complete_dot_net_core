using System;
using Microsoft.EntityFrameworkCore;
using CompleteDotNetCoreWeb.Models;

namespace CompleteDotNetCoreWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Creates the Category table
        public DbSet<Category> Categories { get; set; }
    }
}

