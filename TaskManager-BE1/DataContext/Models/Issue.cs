using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DataContext.Models
{
    public class Issue
    {
        public Issue()  
        {
            Id=Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }

        [Required]
        public string ReporterId { get; set; }
        public string? AssigneeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxLength(50)]
        public Enums.IssueStatus Status { get; set; }

        [MaxLength(20)]
        public Enums.IssuePriority Priority { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Comment> Comments { get; set; }


        [ForeignKey("ReporterId")]
        public User Reporter { get; set; }

        [ForeignKey("AssigneeId")]
        public User Assignee { get; set; }
    }

   
}