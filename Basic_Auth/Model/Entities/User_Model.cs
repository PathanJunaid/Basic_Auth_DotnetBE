using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Basic_Auth.Model.Entities
{
    public enum UserRole
    {
        Admin,
        User
    }

    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }= Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Active { get; set; } = true;
        public UserRole Role { get; set; } = UserRole.User;
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Created once
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<Blog_Model> Blogs { get; set; } = new List<Blog_Model>();
    }

}
