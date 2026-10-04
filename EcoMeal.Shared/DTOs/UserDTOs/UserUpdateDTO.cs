using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Shared.DTOs.UserDTOs;

public class UserUpdateDTO
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; } = "Customer";

    public string? NewPassword { get; set; }
}
