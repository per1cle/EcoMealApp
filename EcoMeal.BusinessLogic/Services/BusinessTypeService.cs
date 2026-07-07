using EcoMeal.DataAccess.Repositories;
using EcoMeal.DataAccess.Entities;
using EcoMeal.Shared.DTOs.BusinessTypeDTOs;
using EcoMeal.BusinessLogic.Services.Interfaces;
namespace EcoMeal.BusinessLogic.Services;

public class BusinessTypeService(IRepository<BusinessType> businessTypeRepository) : IBusinessTypeService
{
    public async Task<List<BusinessTypeGetDTO>> GetAllBusinessTypesAsync()
    {
        var businessTypes = await businessTypeRepository.GetAllAsync();
        return businessTypes.Select(MapToBusinessTypeGetDTO).ToList();
    }
    private static BusinessTypeGetDTO MapToBusinessTypeGetDTO(BusinessType businessType)
    {
        return new BusinessTypeGetDTO
        {
            Id = businessType.Id,
            Name = businessType.Name
        };
    }
}