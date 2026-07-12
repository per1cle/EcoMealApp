using EcoMeal.Shared.DTOs.BusinessDTOs;
namespace EcoMeal.BusinessLogic.Services.Interfaces;

public interface IBusinessService
{
    Task<List<BusinessGetDTO>> GetAllBusinessesAsync();
    Task DeleteBusinessAsync(Guid id);
    Task<BusinessGetDTO> UpdateBusinessAsync(Guid id, BusinessUpdateDTO businessUpdateDTO);
    Task<BusinessGetDTO> AddBusinessAsync(BusinessCreateDTO businessCreateDTO);
    Task<BusinessGetDTO?> GetMyBusinessAsync(Guid userId);
}