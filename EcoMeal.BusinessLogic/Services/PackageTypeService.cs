using EcoMeal.DataAccess.Repositories;
using EcoMeal.DataAccess.Entities;
using EcoMeal.Shared.DTOs.PackageTypeDTOs;
using EcoMeal.BusinessLogic.Services.Interfaces;
namespace EcoMeal.BusinessLogic.Services;

public class PackageTypeService(IRepository<PackageType> packageTypeRepository) : IPackageTypeService
{
    public async Task<List<PackageTypeGetDTO>> GetAllPackageTypesAsync()
    {
        var packageTypes = await packageTypeRepository.GetAllAsync();
        return packageTypes.Select(MapToPackageTypeGetDTO).ToList();
    }
    private static PackageTypeGetDTO MapToPackageTypeGetDTO(PackageType packageType)
    {
        return new PackageTypeGetDTO
        {
            Id = packageType.Id,
            Name = packageType.Name
        };
    }
}