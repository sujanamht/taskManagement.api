using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.api.Models
{
    [Table("projectTbl")]
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; } 
        public User User { get; set; } = null!; // Navigation property to User    
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public ProjectStatus Status { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
    public enum ProjectStatus
    {
        Todo = 1,
        InProgress =2,
        Done =3
    }

    public class ProjectCreateDto
    {
 
        public string ProjTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
    }

    public class ProjectGetDto
    {
        public int ProjectId { get; set; }
        public string ProjTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public ProjectStatus Status { get; set; }
    }

    public class ProjectUpdateDto
    {
        public string ProjTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public ProjectStatus Status { get; set; }
    }

    // This is where we define database rules for the Project table.
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.ProjectId);
            builder.Property(p => p.ProjTitle).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()"); // Automatically set to current UTC time when a new project is created.
            // Define relationship with User
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Projects)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict); //a user cannot be deleted if they still own projects.
        }


    }

}
