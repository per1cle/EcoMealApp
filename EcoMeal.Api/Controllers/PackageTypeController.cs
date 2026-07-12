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

    [HttpPost]
    public async Task<ActionResult<PackageTypeGetDTO>> AddPackageType(PackageTypeCreateDTO packageTypeCreateDTO)
    {
        try
        {
            var packageType = await packageTypeService.AddPackageTypeAsync(packageTypeCreateDTO);
            return CreatedAtAction(nameof(GetPackageTypes), new { id = packageType.Id }, packageType);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the package type: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PackageTypeGetDTO>> UpdatePackageType(Guid id, PackageTypeUpdateDTO packageTypeUpdateDTO)
    {
        try
        {
            var packageType = await packageTypeService.UpdatePackageTypeAsync(id, packageTypeUpdateDTO);
            return Ok(packageType);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the package type: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePackageType(Guid id)
    {
        try
        {
            await packageTypeService.DeletePackageTypeAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}