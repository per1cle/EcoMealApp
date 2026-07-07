using Microsoft.AspNetCore.Mvc;
using EcoMeal.Shared.DTOs.PackageTypeDTOs;
using EcoMeal.BusinessLogic.Services.Interfaces;
namespace EcoMeal.Api.Controllers;

[ApiController]
[Route("api/packagetypes")]
public class PackageTypeController(IPackageTypeService packageTypeService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<PackageTypeGetDTO>>> GetPackageTypes()
    {
        try
        {
            var packageTypes = await packageTypeService.GetAllPackageTypesAsync();
            return Ok(packageTypes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving package types: " + ex.Message);
        }
    }
}