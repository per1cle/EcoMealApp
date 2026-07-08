using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Shared.DTOs.PackageTypeDTOs;

public class PackageTypeUpdateDTO
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Package name is required.")]
    [StringLength(100, ErrorMessage = "Package name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;
}