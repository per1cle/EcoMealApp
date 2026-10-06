namespace EcoMeal.Shared.DTOs.BusinessDTOs;

public class GeocodeResultDTO
{
    public bool Success { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string? DisplayName { get; set; }
    public string? Message { get; set; }
}
