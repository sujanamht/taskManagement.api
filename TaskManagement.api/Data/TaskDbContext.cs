using Microsoft.EntityFrameworkCore;
using TaskManagement.api.Models;
using System.Reflection;

namespace TaskManagement.api.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        // This constructor receives database configuration and gives it to DbContext.
        {
        }

        // This special method lets us customize how our database tables should look
        // and how entities (C# classes) map to tables.
        // Example: we can say "FirstName is required" or "Email must be unique".
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Always call base.OnModelCreating first so EF Core can do its default setup
            base.OnModelCreating(modelBuilder);

            // This line says: "Go look inside this project assembly (code library)
            // and apply all the entity configurations you can find."
            // Example: It will find our "UserEntityConfiguration" class and apply its rules.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).GetTypeInfo().Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }



    }
    }
