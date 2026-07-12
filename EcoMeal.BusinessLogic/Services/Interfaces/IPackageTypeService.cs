namespace EcoMeal.BusinessLogic.Services.Interfaces;

using EcoMeal.Shared.DTOs.PackageTypeDTOs;
public interface IPackageTypeService
{
    Task<List<PackageTypeGetDTO>> GetAllPackageTypesAsync();
    Task<PackageTypeGetDTO> AddPackageTypeAsync(PackageTypeCreateDTO packageTypeCreateDTO);
    Task<PackageTypeGetDTO> UpdatePackageTypeAsync(Guid id, PackageTypeUpdateDTO packageTypeUpdateDTO);
    Task DeletePackageTypeAsync(Guid id);
}
