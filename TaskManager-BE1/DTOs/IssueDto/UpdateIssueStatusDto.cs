using System.ComponentModel.DataAnnotations;
using TaskManager.DataContext;
using TaskManager.DataContext;

namespace TaskManager.DTOs.IssueDto
{
    public class UpdateIssueStatusDto
    {
        [Required]
        public Enums.IssueStatus Status { get; set; }
    }
}