using TaskManager.DataContext;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.DataContext.Models;
using TaskManager.DTOs;
using TaskManager.DTOs.IssueDto;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories.Services
{
    public class IssueRepository : IIssueRepository
    {
        private readonly AppDbContext _context;

        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<IssueResponseDto>> CreateIssueAsync(CreateIssueDto createIssueDto, string userId)
        {
            if (string.IsNullOrWhiteSpace(createIssueDto.Title) || createIssueDto.Title.Length > 255 || string.IsNullOrWhiteSpace(createIssueDto.Description))
                return ApiResponseHelper.Error<IssueResponseDto>(new List<ApiError> {
                 new ApiError{Field = "Validation", Message="Validation error"}
                });

            var issue = new Issue
            {
                Title = createIssueDto.Title,
                Description = createIssueDto.Description,
                AssigneeId = createIssueDto.AssigneeId ?? null,
                Priority = createIssueDto.Priority,
                ReporterId = userId
            };

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.Success<IssueResponseDto>(new IssueResponseDto
            {
                Id = issue.Id,
                Message = "Issue created successfully"
            });
        }
    }
}