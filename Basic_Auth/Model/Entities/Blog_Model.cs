using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Basic_Auth.Model.Entities;

public class Blog_Model
{
    [Key] [Required] 
    public Guid ID { get; set; } = Guid.NewGuid();
    [Required]
    public string Title { get; set; }
    [Required]
    [Column(TypeName = "text")]
    public string Content { get; set; }
    [Required]
    public string Image { get; set; }

    public bool Approved { get; set; } = false;
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
}