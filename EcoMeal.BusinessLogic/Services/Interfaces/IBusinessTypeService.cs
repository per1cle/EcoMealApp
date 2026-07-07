using EcoMeal.Shared.DTOs.BusinessTypeDTOs;
namespace EcoMeal.BusinessLogic.Services.Interfaces;

public interface IBusinessTypeService
{
    Task<List<BusinessTypeGetDTO>> GetAllBusinessTypesAsync();
}