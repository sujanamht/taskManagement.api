using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.api.Models
{
    [Table("taskTbl")]
    public class TaskItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required, MaxLength(100)]
        public string TaskTitle { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public int ProjectId { get; set; }   // Foreign key to Project
        public Project Project { get; set; } = null!; // Navigation property to Project
       
       
        public int UserId { get; set; }// Foreign key to User
        public User User { get; set; } = null!; // Navigation property to User

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set; }


        public TaskStatus Status { get; set; }

        public Priority Priority { get; set; }
    }

    public enum TaskStatus
    {
        Todo = 1,
        InProgress = 2,
        Done = 3
    }

    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
