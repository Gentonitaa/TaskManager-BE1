using TaskManager.DataContext;
using TaskManager.DTOs.CommentDto;

namespace TaskManager.DTOs.IssueDto
{
    public class GetIssueByIdDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReporterId { get; set; }
        public string AssigneeId { get; set; }
        public Enums.IssueStatus Status { get; set; }
        public Enums.IssuePriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CommentDto.CommentDto> Comments { get; set; }

    }
}