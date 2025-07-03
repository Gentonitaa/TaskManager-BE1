using Microsoft.AspNetCore.Identity;

namespace TaskManager.DTOs.UserDto
{
    public class GetAllUsersDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }


}