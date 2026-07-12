using EcoMeal.Shared.DTOs.BusinessTypeDTOs;
namespace EcoMeal.BusinessLogic.Services.Interfaces;

public interface IBusinessTypeService
{
    Task<List<BusinessTypeGetDTO>> GetAllBusinessTypesAsync();
    Task<BusinessTypeGetDTO> AddBusinessTypeAsync(BusinessTypeCreateDTO businessTypeCreateDTO);
    Task<BusinessTypeGetDTO> UpdateBusinessTypeAsync(Guid id, BusinessTypeUpdateDTO businessTypeUpdateDTO);
    Task DeleteBusinessTypeAsync(Guid id);
}