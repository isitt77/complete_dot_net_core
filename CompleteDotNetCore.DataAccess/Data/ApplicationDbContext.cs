using System;
using Microsoft.EntityFrameworkCore;
using CompleteDotNetCore.Models;

namespace CompleteDotNetCore.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Creates the Category table
        public DbSet<Category> Categories { get; set; }

        // Creates the CoverType table
        public DbSet<CoverType> CoverTypes { get; set; }
    }
}

