using TaskManager.DataContext;

namespace TaskManager.DTOs.IssueDto
{
    public class SearchIssueDto
    {
        public string Text { get; set; }

        public List<string> UserIds { get; set; }

        public Enums.IssueStatus Status { get; set; }
    }
}