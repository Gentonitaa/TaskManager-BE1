using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DataContext.Models
{
    public class Comment
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string IssueId { get; set; }
        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [ForeignKey("IssueId")]
        public Issue Issue { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}