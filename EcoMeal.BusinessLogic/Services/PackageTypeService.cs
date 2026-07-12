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

    public async Task<PackageTypeGetDTO> AddPackageTypeAsync(PackageTypeCreateDTO packageTypeCreateDTO)
    {
        var packageType = new PackageType
        {
            Name = packageTypeCreateDTO.Name
        };

        var addedPackageType = await packageTypeRepository.AddAsync(packageType);
        return MapToPackageTypeGetDTO(addedPackageType);
    }

    public async Task<PackageTypeGetDTO> UpdatePackageTypeAsync(Guid id, PackageTypeUpdateDTO packageTypeUpdateDTO)
    {
        var packageType = await packageTypeRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"PackageType with ID {id} not found.");

        packageType.Name = packageTypeUpdateDTO.Name;

        var updatedPackageType = await packageTypeRepository.UpdateAsync(packageType);
        return MapToPackageTypeGetDTO(updatedPackageType);
    }

    public async Task DeletePackageTypeAsync(Guid id)
    {
        var packageType = await packageTypeRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"PackageType with ID {id} not found.");
        await packageTypeRepository.DeleteAsync(packageType);
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