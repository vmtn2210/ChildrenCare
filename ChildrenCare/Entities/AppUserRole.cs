using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ChildrenCare.Entities;

public class AppUserRole: IdentityUserRole<int>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}