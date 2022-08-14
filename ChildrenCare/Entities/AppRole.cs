using Microsoft.AspNetCore.Identity;

namespace ChildrenCare.Entities;

public class AppRole : IdentityRole<int>
{
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
}