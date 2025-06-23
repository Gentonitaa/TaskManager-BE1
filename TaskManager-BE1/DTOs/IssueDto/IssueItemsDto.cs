
using TaskManager.DataContext;

namespace TaskManager.DTOs.IssueDto

{
    public class IssueItemsDto
    {
        public Enums.IssueStatus Status { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string AssigneeId { get; set; }
        public string FullName { get; set; }
    }
}