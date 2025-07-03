using Microsoft.AspNetCore.Identity;

namespace TaskManager.DataContext.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? LastLogin { get; internal set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}