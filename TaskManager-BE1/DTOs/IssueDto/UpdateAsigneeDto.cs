using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs.IssueDto
{
    public class UpdateAsigneeDto
    {
        [Required]
        public string AssigneeId { get; set; }
    }
}
