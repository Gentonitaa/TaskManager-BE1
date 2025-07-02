namespace TaskManager.DTOs.UserDto
{
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}