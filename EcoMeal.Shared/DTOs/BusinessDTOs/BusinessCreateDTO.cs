using System.ComponentModel.DataAnnotations;
namespace EcoMeal.Shared.DTOs.BusinessDTOs;

public class BusinessCreateDTO
{
    [Required(ErrorMessage = "Business name is required.")]
    [MaxLength(255, ErrorMessage = "Business name cannot exceed 255 characters.")]
    public string Name { get; set; } = null!;

    [MaxLength(1000, ErrorMessage = "Business description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Business address is required.")]
    [MaxLength(255, ErrorMessage = "Business address cannot exceed 255 characters.")]
    public string Address { get; set; } = null!;

    private string? _imageUrl;

    //[Url(ErrorMessage = "Invalid URL format for ImageUrl.")]
    public string? ImageUrl
    {
        get => _imageUrl;
        set => _imageUrl = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    [Required(ErrorMessage = "Business type is required.")]
    public Guid BusinessTypeId { get; set; }


}