using Microsoft.AspNetCore.Mvc;
using EcoMeal.Shared.DTOs.PackageDTOs;
using EcoMeal.BusinessLogic.Services.Interfaces;
namespace EcoMeal.Api.Controllers;

[ApiController]
[Route("api/packages")]
public class PackageController(IPackageService packageService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<PackageGetDTO>>> GetPackages()
    {
        try
        {
            var packages = await packageService.GetAllPackagesAsync();
            return Ok(packages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving packages: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<PackageGetDTO>> AddPackage(PackageCreateDTO packageCreateDTO)
    {
        try
        {
            var package = await packageService.AddPackageAsync(packageCreateDTO);
            return CreatedAtAction(nameof(GetPackages), new { id = package.Id }, package);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the package: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PackageGetDTO>> UpdatePackage(Guid id, PackageUpdateDTO packageUpdateDTO)
    {
        try
        {
            var package = await packageService.UpdatePackageAsync(id, packageUpdateDTO);
            return Ok(package);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the package: " + ex.Message);
        }
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePackage(Guid id)
    {
        try
        {
            await packageService.DeletePackageAsync(id);
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