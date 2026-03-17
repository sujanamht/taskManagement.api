using Microsoft.EntityFrameworkCore;
using TaskManagement.api.Models;

namespace TaskManagement.api.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        // This constructor receives database configuration and gives it to DbContext.
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }



    }
    }
