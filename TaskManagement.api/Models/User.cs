using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaskManagement.api.Models
{
    [Table("userTbl")]
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;   
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } 
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    }
    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        Employee = 3,
       
    } 

    public class UserGetDto
    {
        public int UserId { get; set; } // We expose ID so frontend knows which user
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Notice: No Password → we should NEVER send passwords to frontend.
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserCreateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class UserUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }

    // This is where we define database rules for the User table.
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Every table needs a primary key (here: UserId)
            builder.HasKey(x => x.UserId);

            // FirstName must NOT be empty, max length 100
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            // LastName must NOT be empty, max length 100
            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // Email must NOT be empty, max length 255
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            // Password must NOT be empty, max length 255
            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
