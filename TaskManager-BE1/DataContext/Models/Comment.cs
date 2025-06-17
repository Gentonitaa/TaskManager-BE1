using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DataContext.Models
{
    public class Comments
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid IssueId { get; set; }
        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [ForeignKey("IssueId")]
        public Issue Issue { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}