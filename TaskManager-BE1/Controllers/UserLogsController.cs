using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLogsController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public UserLogsController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLog(UserLog log)
        {
            log.Id = null;                // Vendos Id=null që MongoDB ta gjenerojë automatikisht
            log.Timestamp = DateTime.UtcNow;
            await _mongoDbService.CreateLogAsync(log);
            return Ok("Log created.");
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _mongoDbService.GetAllLogsAsync();
            return Ok(logs);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredLogs([FromQuery] string? userId, [FromQuery] string? action)
        {
            var logs = await _mongoDbService.GetFilteredLogsAsync(userId, action);
            return Ok(logs);
        }

        // Ky endpoint simulon login dhe regjistron log automatikisht
        [HttpPost("simulate-login")]
        public async Task<IActionResult> SimulateLogin([FromBody] string userId)
        {
            var log = new UserLog
            {
                UserId = userId,
                Action = "Login",
                Description = $"User with ID {userId} has logged in.",
                Timestamp = DateTime.UtcNow
            };

            await _mongoDbService.CreateLogAsync(log);

            return Ok("Login simulated and log saved.");
        }
    }
}
