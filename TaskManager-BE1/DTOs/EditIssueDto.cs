using System.ComponentModel.DataAnnotations;
using TaskManager.DataContext;

namespace TaskManager.DTOs.IssueDto
{
    public class EditIssueDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string? AssigneeId { get; set; }

        [Required]
        public Enums.IssueStatus Status { get; set; }

        [Required]
        public Enums.IssuePriority Priority { get; set; }
    }
}