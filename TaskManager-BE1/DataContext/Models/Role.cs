using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole
{
    public virtual ICollection<UserRole> UserRoles { get; set; }
}
