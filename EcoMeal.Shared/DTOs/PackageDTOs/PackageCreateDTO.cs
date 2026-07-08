using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Shared.DTOs.PackageDTOs;

public class PackageCreateDTO
{
    [Required(ErrorMessage = "Package name is required.")]
    [MaxLength(255, ErrorMessage = "Package name cannot exceed 255 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Business ID is required.")]
    public Guid BusinessId { get; set; }

    [Required(ErrorMessage = "Package type ID is required.")]
    public Guid PackageTypeId { get; set; }

    [MaxLength(1000, ErrorMessage = "Package description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Pickup start time is required.")]
    public DateTime PickupStart { get; set; }

    [Required(ErrorMessage = "Pickup end time is required.")]
    public DateTime PickupEnd { get; set; }

    //[Url(ErrorMessage = "Invalid URL format for ImageUrl.")]
    public string? ImageUrl { get; set; }
}