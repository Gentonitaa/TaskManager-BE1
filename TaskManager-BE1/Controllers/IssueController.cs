using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.DTOs;
using TaskManager.DTOs.IssueDto;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IIssueRepository _issueRepository;

        public IssueController(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueDto createIssueDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponseHelper.Failure<string>("User is not authenticated"));

            var result = await _issueRepository.CreateIssueAsync(createIssueDto, userId);

            if (!result.Status)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetIssueById), new { id = result.Data.Id }, result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditIssue(string id, [FromBody] EditIssueDto editIssueDto)
        {
            var result = await _issueRepository.EditIssueAsync(id, editIssueDto);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIssueById(string id)
        {
            var result = await _issueRepository.GetIssueByIdAsync(id);

            if (!result.Status)
                return NotFound(result);

            return Ok(result);
        }
    }
}