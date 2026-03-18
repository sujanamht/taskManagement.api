using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.api.Models
{ 
    public class TaskItem
    {
        public int TaskItemId { get; set; }

        public string TaskTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int ProjectId { get; set; }   // Foreign key to Project
        public Project Project { get; set; } = null!; // Navigation property to Project
       
       
        public int UserId { get; set; }// Foreign key to User
        public User User { get; set; } = null!; // Navigation property to User

        public DateTime CreatedDate { get; set; } //start date is supposedly create date

        public DateTime DueDate { get; set; }


        public TaskStatus Status { get; set; }

        public Priority Priority { get; set; }
    }

    public enum TaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Done = 2
    }

    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public class TaskItemCreateDto
    {
        public string TaskTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int UserId { get; set; }  //assigned user
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public Priority Priority { get; set; }
    }

    public class TaskItemGetDto
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }  // start date
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public Priority Priority { get; set; }
    }
    public class TaskItemUpdateDto {
        public string TaskTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public Priority Priority { get; set; }
    }

    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(t => t.TaskItemId); // Primary key

            builder.Property(t => t.TaskTitle)
                .IsRequired()
                .HasMaxLength(100); // Task title is required and has a maximum length of 100 characters

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(2000); // Task description is required and has a maximum length of 2000 characters

            builder.Property(t => t.ProjectId).IsRequired();  

            builder.Property(t => t.UserId).IsRequired();

            builder.Property(t => t.Status)
                .IsRequired()
                .HasDefaultValue(TaskStatus.Todo);

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasDefaultValue(Priority.Low);

            builder.Property(t => t.CreatedDate)
                   .HasDefaultValueSql("GETUTCDATE()");

            // A user cannot be deleted if they still have tasks assigned to them.
            builder.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // A project cannot be deleted if it still has tasks assigned to it.
            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }   
}
