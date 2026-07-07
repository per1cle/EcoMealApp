namespace EcoMeal.BusinessLogic.Services.Interfaces;

using EcoMeal.Shared.DTOs.PackageDTOs;
public interface IPackageService
{
    Task<List<PackageGetDTO>> GetAllPackagesAsync();
    Task<PackageGetDTO> UpdatePackageAsync(Guid id, PackageUpdateDTO packageUpdateDTO);
    Task DeletePackageAsync(Guid id);
    Task<PackageGetDTO> AddPackageAsync(PackageCreateDTO packageCreateDTO);
}