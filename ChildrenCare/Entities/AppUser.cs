using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;

namespace ChildrenCare.Entities;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser<int>
{
    [Required(ErrorMessage = "Please enter your name")]
    [MinLength(3), MaxLength(100)]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Please select your gender")]
    public bool Gender { get; set; }

    [Required]
    public bool IsPotentialCustomer { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; }

    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    
    public virtual ICollection<Prescription> PrescriptionCustomerAccounts { get; set; }

    public virtual ICollection<Prescription> PrescriptionAuthorAccounts { get; set; }
    
}

