using EcoMeal.DataAccess.Repositories;
using EcoMeal.DataAccess.Entities;
using EcoMeal.BusinessLogic.Services.Interfaces;
using EcoMeal.Shared.DTOs.BusinessDTOs;
namespace EcoMeal.BusinessLogic.Services;

public class BusinessService(IRepository<Business> businessRepository) : IBusinessService
{
    public async Task<List<BusinessGetDTO>> GetAllBusinessesAsync()
    {
        var businesses = await businessRepository.GetAllAsync();
        return businesses.Select(MaptoBusinessGetDTO).ToList();
    }
    public async Task<BusinessGetDTO> AddBusinessAsync(BusinessCreateDTO businessCreateDTO)
    {
        var business = new Business
        {
            Name = businessCreateDTO.Name,
            UserId = businessCreateDTO.UserId,
            Description = businessCreateDTO.Description,
            Address = businessCreateDTO.Address,
            ImageUrl = businessCreateDTO.ImageUrl,
            BusinessTypeId = businessCreateDTO.BusinessTypeId
        };

        var addedBusiness = await businessRepository.AddAsync(business);
        return MaptoBusinessGetDTO(addedBusiness);
    }

    public async Task<BusinessGetDTO> UpdateBusinessAsync(Guid id, BusinessUpdateDTO businessUpdateDTO)
    {
        var business = await businessRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Business with ID {id} not found.");

        business.Name = businessUpdateDTO.Name;
        business.Description = businessUpdateDTO.Description;
        business.Address = businessUpdateDTO.Address;
        business.ImageUrl = businessUpdateDTO.ImageUrl;
        business.BusinessTypeId = businessUpdateDTO.BusinessTypeId;

        var updatedBusiness = await businessRepository.UpdateAsync(business);
        return MaptoBusinessGetDTO(updatedBusiness);
    }

    public async Task DeleteBusinessAsync(Guid id)
    {
        var business = await businessRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Business with ID {id} not found.");
        await businessRepository.DeleteAsync(business);
    }

    public async Task<BusinessGetDTO?> GetMyBusinessAsync(Guid userId)
    {
        var businesses = await businessRepository.GetAllAsync();
        var myBusiness = businesses.FirstOrDefault(b => b.UserId == userId);
        if (myBusiness == null)
        {
            return null;
        }
        return MaptoBusinessGetDTO(myBusiness);
    }
    private static BusinessGetDTO MaptoBusinessGetDTO(Business business)
    {
        return new BusinessGetDTO
        {
            Id = business.Id,
            Name = business.Name,
            UserId = business.UserId,
            Description = business.Description ?? string.Empty,
            Address = business.Address ?? string.Empty,
            ImageUrl = business.ImageUrl ?? string.Empty,
            BusinessTypeId = business.BusinessTypeId
        };
    }
}