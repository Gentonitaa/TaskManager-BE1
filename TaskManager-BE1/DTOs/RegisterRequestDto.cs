using System.ComponentModel.DataAnnotations;

namespace TaskManager_BE1.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        public string Password { get; set; }

    }
}