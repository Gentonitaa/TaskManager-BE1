using TaskManager.DataContext; 
using TaskManager.DTOs;
using TaskManager.DTOs.IssueDto;

namespace TaskManager.Repositories.Interfaces
{
    public interface IIssueRepository
    {
        Task<ApiResponse<IssueResponseDto>> CreateIssueAsync(CreateIssueDto createIssueDto, string id);
        Task<ApiResponse<IssueResponseDto>> EditIssueAsync(string issueId, EditIssueDto editIssueDto);
        Task<ApiResponse<string>> DeleteIssueAsync(string issueId);
        Task<ApiResponse<GetIssueByIdDto>> GetIssueByIdAsync(string id);
        Task<ApiResponse<GroupedIssueDto>> GetAllIssuesAsync(SearchIssueDto searchIssueDto);
        Task<ApiResponse<string>> UpdateIssueStatusAsync(string issueId, Enums.IssueStatus status);
        Task<ApiResponse<string>> ChangeIssueAssigneeAsync(string issueId, string assigneeId);
        Task<ApiResponse<UserIssueNumberDto>> UserIssueCountAsync(string userId);
    }
}