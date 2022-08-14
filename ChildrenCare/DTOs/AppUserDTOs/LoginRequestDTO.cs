using System.ComponentModel.DataAnnotations;

namespace ChildrenCare.DTOs.AppUserDTOs;

public class LoginRequestDTO
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }

    [Required]
    public bool RememberMe { get; set; }
}