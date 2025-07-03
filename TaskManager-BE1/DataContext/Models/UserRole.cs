using Microsoft.AspNetCore.Identity;
using TaskManager.DataContext.Models;

public class UserRole : IdentityUserRole<string>
{
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}
