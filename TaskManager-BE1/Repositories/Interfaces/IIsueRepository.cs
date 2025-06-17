using TaskManager.DTOs;
using TaskManager.DataContext.Models; 
using TaskManager.DTOs;
using TaskManager.DTOs.IssueDto;

namespace TaskManager.Repositories.Interfaces
{
    public interface IIssueRepository
    {
        Task<ApiResponse<IssueResponseDto>> CreateIssueAsync(CreateIssueDto createIssueDto, string id);

    }
}