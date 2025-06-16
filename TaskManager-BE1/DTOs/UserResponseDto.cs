namespace TaskManager.DTOs
{
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Token { get; set; }
    }
}