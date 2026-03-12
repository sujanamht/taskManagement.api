using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.api.Models
{
    [Table("projectTbl")]
    public class Project
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }

        [Required, MaxLength(100)]
        public string ProjTitle { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
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
}
