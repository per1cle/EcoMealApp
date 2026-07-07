namespace EcoMeal.BusinessLogic.Services.Interfaces;

using EcoMeal.Shared.DTOs.PackageTypeDTOs;
public interface IPackageTypeService
{
    Task<List<PackageTypeGetDTO>> GetAllPackageTypesAsync();
}
