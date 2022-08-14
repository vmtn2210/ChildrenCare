using System.ComponentModel.DataAnnotations;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;

namespace ChildrenCare.DTOs.ReservationDTOs;

public class UpdateReservationContactDTO
{
    [Required] public int Id { get; set; }

    [Required(ErrorMessage = "Please enter name")]
    [MinLength(3), MaxLength(100)]
    public string? CustomerName { get; set; }

    [Required(ErrorMessage = "Please select gender")]
    public int CustomerGender { get; set; }

    [Required(ErrorMessage = "Please enter phone number")]
    [StringLength(10)]
    [RegularExpression(GlobalVariables.PhoneNumberRegex, ErrorMessage = "Must be 10 digits")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Please enter address")]
    public string? Address { get; set; }

    public IEnumerable<SelectListItem>? GenderList { get; set; }
    [MaxLength(500)] public string? Notes { get; set; }

    [Required(ErrorMessage = "Please enter date")]
    [DateGreaterThan]
    public DateTime? PreservedDate { get; set; }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var preservedDate = (DateTime)value;

            var dateNow = DateTime.Now;

            if (preservedDate >= dateNow)
            {
                return ValidationResult.Success;
            }
            else
            {
                preservedDate = DateTime.Now;
                return new ValidationResult("Preserved Date cannot be in the past");
            }
        }
    }
}