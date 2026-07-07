using EcoMeal.DataAccess.Repositories;
using EcoMeal.DataAccess.Entities;
using EcoMeal.BusinessLogic.Services.Interfaces;
using EcoMeal.Shared.DTOs.PackageDTOs;
namespace EcoMeal.BusinessLogic.Services;

public class PackageService(IRepository<Package> packageRepository) : IPackageService
{
    public async Task<List<PackageGetDTO>> GetAllPackagesAsync()
    {
        var packages = await packageRepository.GetAllAsync();
        return packages.Select(MaptoPackageGetDTO).ToList();
    }
    public async Task<PackageGetDTO> AddPackageAsync(PackageCreateDTO packageCreateDTO)
    {
        var package = new Package
        {
            Name = packageCreateDTO.Name,
            Description = packageCreateDTO.Description,
            BusinessId = packageCreateDTO.BusinessId,
            PackageTypeId = packageCreateDTO.PackageTypeId,
            Price = packageCreateDTO.Price,
            Quantity = packageCreateDTO.Quantity,
            PickupStart = packageCreateDTO.PickupStart,
            PickupEnd = packageCreateDTO.PickupEnd,
            ImageUrl = packageCreateDTO.ImageUrl
        };

        var addedPackage = await packageRepository.AddAsync(package);
        return MaptoPackageGetDTO(addedPackage);
    }

    public async Task<PackageGetDTO> UpdatePackageAsync(Guid id, PackageUpdateDTO packageUpdateDTO)
    {
        var package = await packageRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Package with ID {id} not found.");

        package.Name = packageUpdateDTO.Name;
        package.Description = packageUpdateDTO.Description;
        package.BusinessId = packageUpdateDTO.BusinessId;
        package.PackageTypeId = packageUpdateDTO.PackageTypeId;
        package.Price = packageUpdateDTO.Price;
        package.Quantity = packageUpdateDTO.Quantity;
        package.PickupStart = packageUpdateDTO.PickupStart;
        package.PickupEnd = packageUpdateDTO.PickupEnd;
        package.ImageUrl = packageUpdateDTO.ImageUrl;

        var updatedPackage = await packageRepository.UpdateAsync(package);
        return MaptoPackageGetDTO(updatedPackage);
    }

    public async Task DeletePackageAsync(Guid id)
    {
        var package = await packageRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Package with ID {id} not found.");
        await packageRepository.DeleteAsync(package);
    }
    private static PackageGetDTO MaptoPackageGetDTO(Package package)
    {
        return new PackageGetDTO
        {
            Id = package.Id,
            Name = package.Name,
            Description = package.Description ?? string.Empty,
            BusinessId = package.BusinessId,
            PackageTypeId = package.PackageTypeId,
            Price = package.Price,
            Quantity = package.Quantity,
            PickupStart = package.PickupStart,
            PickupEnd = package.PickupEnd,
            ImageUrl = package.ImageUrl ?? string.Empty
        };
    }
}