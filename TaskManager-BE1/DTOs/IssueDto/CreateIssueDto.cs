using System.ComponentModel.DataAnnotations;
using TaskManager.DataContext;
using TaskManager.DataContext;

namespace TaskManager.DTOs.IssueDto
{
    public class CreateIssueDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string? AssigneeId { get; set; }

        public Enums.IssuePriority Priority { get; set; }
        public Enums.IssueStatus Status { get; set; } = Enums.IssueStatus.ToDo;

    }
}