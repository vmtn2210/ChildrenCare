using System.ComponentModel.DataAnnotations;
using ChildrenCare.Entities;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.OpenApi.Extensions;

namespace ChildrenCare.DTOs.AppUserDTOs;

public class RegisterRequestDTO
{
    public RegisterRequestDTO()
    {
        GenderList = new List<SelectListItem>();
    }

    [Required]
    [MinLength(3), MaxLength(100)]
    [Display(Name = "FullName")]
    public string? FullName { get; set; }
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    [StringLength(10)]
    [RegularExpression(GlobalVariables.PhoneNumberRegex, ErrorMessage = "Must be 10 digits")]
    public string? PhoneNumber { get; set; }

    public int Gender { get; set; }

    public IEnumerable<SelectListItem> GenderList { get; set; }

    public AppUser MapToNewUser()
    {
        var result = new AppUser()
        {
            IsPotentialCustomer = false,
            PhoneNumber = PhoneNumber,
            FullName = FullName,
            Gender = true
        };

        if (Gender == (int)GlobalVariables.GenderType.Female)
        {
            result.Gender = false;
        }

        return result;
    }
}