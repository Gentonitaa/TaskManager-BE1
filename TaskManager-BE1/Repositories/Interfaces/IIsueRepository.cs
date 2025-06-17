using TaskManager.DataContext;
using TaskManager.DataContext.Models; 
using TaskManager.DTOs;
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

        Task<ApiResponse<List<IssueItemsDto>>> GetAllIssuesAsync();
        Task<ApiResponse<string>> UpdateIssueStatusAsync(string issueId, Enums.IssueStatus status);
    }
}