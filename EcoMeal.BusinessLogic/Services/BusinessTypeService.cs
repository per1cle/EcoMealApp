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

    public async Task<BusinessTypeGetDTO> AddBusinessTypeAsync(BusinessTypeCreateDTO businessTypeCreateDTO)
    {
        var businessType = new BusinessType
        {
            Name = businessTypeCreateDTO.Name
        };

        var addedBusinessType = await businessTypeRepository.AddAsync(businessType);
        return MapToBusinessTypeGetDTO(addedBusinessType);
    }

    public async Task<BusinessTypeGetDTO> UpdateBusinessTypeAsync(Guid id, BusinessTypeUpdateDTO businessTypeUpdateDTO)
    {
        var businessType = await businessTypeRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"BusinessType with ID {id} not found.");

        businessType.Name = businessTypeUpdateDTO.Name;

        var updatedBusinessType = await businessTypeRepository.UpdateAsync(businessType);
        return MapToBusinessTypeGetDTO(updatedBusinessType);
    }

    public async Task DeleteBusinessTypeAsync(Guid id)
    {
        var businessType = await businessTypeRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"BusinessType with ID {id} not found.");
        await businessTypeRepository.DeleteAsync(businessType);
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