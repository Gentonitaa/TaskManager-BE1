namespace TaskManager.DTOs.IssueDto
{
    public class GroupedIssueDto
    {
        public List<IssueItemsDto> ToDo { get; set; }

        public List<IssueItemsDto> Inprogress { get; set; }

        public List<IssueItemsDto> Review { get; set; }

        public List<IssueItemsDto> Done { get; set; }
    }
}