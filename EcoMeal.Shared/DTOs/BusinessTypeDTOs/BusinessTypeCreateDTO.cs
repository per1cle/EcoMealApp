using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Shared.DTOs.BusinessTypeDTOs;

public class BusinessTypeCreateDTO
{
    [Required(ErrorMessage = "Business type name is required.")]
    [MaxLength(255, ErrorMessage = "Business type name cannot exceed 255 characters.")]
    public string Name { get; set; } = null!;
}