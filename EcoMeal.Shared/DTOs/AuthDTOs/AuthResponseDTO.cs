namespace EcoMeal.Shared.DTOs.AuthDTOs;

public class AuthResponseDTO
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
}