using Microsoft.EntityFrameworkCore;
using TaskManager.DataContext;
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
            if (string.IsNullOrWhiteSpace(createIssueDto.Title) || !Enum.IsDefined(typeof(Enums.IssuePriority), createIssueDto.Priority))
            {
                return ApiResponseHelper.Error<IssueResponseDto>(new List<ApiError>
                {
                    new ApiError {Field= "Validation", Message= "Validation Error"}
                }, "Validation Failed");
            }

            if (!string.IsNullOrWhiteSpace(createIssueDto.AssigneeId))
            {
                var assignee = await _context.Users.FindAsync(createIssueDto.AssigneeId);
                if (assignee == null)
                {
                    return ApiResponseHelper.Error<IssueResponseDto>(new List<ApiError>
                    {
                         new ApiError { Field = "AssigneeId", Message = "Assignee not found" }
                    });
                }
            }



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



        public async Task<ApiResponse<IssueResponseDto>> EditIssueAsync(string issueId, EditIssueDto editIssueDto)
        {
            var issue = await _context.Issues.FindAsync(issueId);

            if (issue == null)
            {
                return ApiResponseHelper.Error<IssueResponseDto>(new List<ApiError> {
                 new ApiError{Field = "Issue", Message="Issue not found"}
                });
            }

            if (string.IsNullOrEmpty(editIssueDto.Title) || editIssueDto.Title.Length > 255 || !Enum.IsDefined(typeof(Enums.IssueStatus), editIssueDto.Status) || !Enum.IsDefined(typeof(Enums.IssuePriority), editIssueDto.Priority))
            {
                return ApiResponseHelper.Error<IssueResponseDto>(new List<ApiError> {
                 new ApiError{Field = "Validation", Message="Invalid Data"}
                });
            }

            //assigneId
            if (!string.IsNullOrWhiteSpace(editIssueDto.AssigneeId))
            {
                var assignee = await _context.Users.FindAsync(editIssueDto.AssigneeId);
                if (assignee == null)
                {
                    return ApiResponseHelper.Error<IssueResponseDto>(new List<ApiError> {
                    new ApiError{Field = "AssigneeId", Message="Assignee not found"}
                });
                }
            }

            issue.Title = editIssueDto.Title;
            issue.Description = editIssueDto.Description;
            issue.AssigneeId = editIssueDto.AssigneeId;
            issue.Status = editIssueDto.Status;
            issue.Priority = editIssueDto.Priority;
            issue.UpdatedAt = DateTime.UtcNow;

            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.Success<IssueResponseDto>(new IssueResponseDto
            {
                Id = issue.Id,
                Message = "Issue updated successfully"
            });

        }

        public async Task<ApiResponse<string>> DeleteIssueAsync(string issueId)
        {
            var issue = await _context.Issues.FindAsync(issueId);

            if (issue == null)
            {
                return ApiResponseHelper.Error<string>(new List<ApiError> {
                 new ApiError{Field = "Issue", Message="Issue not found"}
                });
            }

            if (issue.IsDeleted)
            {
                return ApiResponseHelper.Error<string>(new List<ApiError> {
                 new ApiError{Field = "Issue", Message="Issue already deleted"}
                });
            }

            issue.IsDeleted = true;
            issue.UpdatedAt = DateTime.UtcNow;

            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.Success("Issue Deleted Successfully");
        }

        public async Task<ApiResponse<GetIssueByIdDto>> GetIssueByIdAsync(string id)
        {
            var issue = await _context.Issues.Include(i => i.Comments).ThenInclude(c => c.Author).FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

            if (issue == null)
            {
                return ApiResponseHelper.Error<GetIssueByIdDto>(new List<ApiError>
                {
                    new ApiError {Field = "Issue", Message = "Issue not found"}
                });
            }

            var response = new GetIssueByIdDto
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                ReporterId = issue.ReporterId,
                AssigneeId = issue.AssigneeId,
                Status = issue.Status,
                Priority = issue.Priority,
                CreatedAt = issue.CreatedAt,
                UpdatedAt = issue.UpdatedAt ?? default(DateTime),
                Comments = issue.Comments.Select(c => new DTOs.CommentDto.CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    AuthorName = $"{c.Author.FirstName} {c.Author.LastName}",
                    CreatedAt = c.CreatedAt
                }).ToList()
            };

            return ApiResponseHelper.Success(response);
        }

        public async Task<ApiResponse<GroupedIssueDto>> GetAllIssuesAsync()
        {
            var issues = await _context.Issues.Where(i => !i.IsDeleted).ToListAsync();

        var groupedIssues = new GroupedIssueDto
        {
            ToDo = issues
            .Where(i => i.Status == Enums.IssueStatus.ToDo)
            .Select(i => new IssueItemsDto { Id = i.Id, Title = i.Title }).ToList(),

            Inprogress = issues
            .Where(i => i.Status == Enums.IssueStatus.Inprogress)
            .Select(i => new IssueItemsDto { Id = i.Id, Title = i.Title }).ToList(),

            Review = issues
            .Where(i => i.Status == Enums.IssueStatus.Review)
            .Select(i => new IssueItemsDto { Id = i.Id, Title = i.Title }).ToList(),

            Done = issues
            .Where(i => i.Status == Enums.IssueStatus.Done)
            .Select(i => new IssueItemsDto { Id = i.Id, Title = i.Title }).ToList()
        };

            return ApiResponseHelper.Success(groupedIssues);
        }
        
        public async Task<ApiResponse<string>> UpdateIssueStatusAsync(string issueId, Enums.IssueStatus status)
        {
            var issue = await _context.Issues.FindAsync(issueId);

            if (issue == null || issue.IsDeleted)
            {
                return ApiResponseHelper.Error<string>(new List<ApiError>
                {
                    new ApiError {Field = "Issue", Message = "Issue not found"}
});
            }

            if (!Enum.IsDefined(typeof(Enums.IssueStatus), status))
            {
                return ApiResponseHelper.Error<string>(new List<ApiError>
                {
                    new ApiError{ Field= "Status", Message = "Status Not Valid"}
                });
            }

            issue.Status = status;

            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.Success("Issue Status updated successfully");
        }
    }
}