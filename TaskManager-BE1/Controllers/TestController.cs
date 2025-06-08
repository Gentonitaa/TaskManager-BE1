using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Services;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(IAuthRepository authRepository) : ControllerBase
    {

        private readonly IAuthRepository _authRepository = authRepository;


        [HttpGet("GetAuth")]
        public IActionResult TestAuth()
        {

            return Ok("Authorized");

        }
    }
}