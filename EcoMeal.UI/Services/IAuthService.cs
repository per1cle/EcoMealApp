using EcoMeal.Shared.DTOs.AuthDTOs;

namespace EcoMeal.UI.Services;

public interface IAuthService
{
    Task<AuthResponseDTO> Login(LoginDTO loginModel);
    Task<AuthResponseDTO> Register(RegisterDTO registerModel);
    Task<AuthResponseDTO> RegisterBusiness(RegisterDTO registerModel);
    Task Logout();
}