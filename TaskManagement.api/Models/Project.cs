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
                                                // 
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public ProjectStatus Status { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
    public enum ProjectStatus
    {
        Todo = 0,
        InProgress =1,
        Completed =2
    }

    public class ProjectCreateDto
    {
 
        public string ProjTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }


    }

    public class ProjectGetDto
    {
        public int ProjectId { get; set; }
        public string ProjTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
            builder.HasKey(p => p.ProjectId); // Set ProjectId as the primary key.

            builder.Property(p => p.ProjTitle)
                   .IsRequired()
                   .HasMaxLength(100); // ProjTitle is required and has a maximum length of 100 characters.

            builder.Property(p => p.Description)
                   .HasMaxLength(500); // maximum length of 500 characters.

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()"); // Automatically set to current UTC time when a new project is created.

            builder.Property(p => p.Status)
                   .HasDefaultValue(ProjectStatus.Todo);

            // Define relationship with User
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Projects)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict); //a user cannot be deleted if they still own projects.
        }


    }

}
