using Microsoft.AspNetCore.Mvc;
using EcoMeal.Shared.DTOs.BusinessTypeDTOs;
using EcoMeal.BusinessLogic.Services.Interfaces;
namespace EcoMeal.Api.Controllers;

[ApiController]
[Route("api/businesstypes")]
public class BusinessTypeController(IBusinessTypeService businessTypeService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<BusinessTypeGetDTO>>> GetBusinessTypes()
    {
        try
        {
            var businessTypes = await businessTypeService.GetAllBusinessTypesAsync();
            return Ok(businessTypes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving business types: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<BusinessTypeGetDTO>> AddBusinessType(BusinessTypeCreateDTO businessTypeCreateDTO)
    {
        try
        {
            var businessType = await businessTypeService.AddBusinessTypeAsync(businessTypeCreateDTO);
            return CreatedAtAction(nameof(GetBusinessTypes), new { id = businessType.Id }, businessType);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the business type: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BusinessTypeGetDTO>> UpdateBusinessType(Guid id, BusinessTypeUpdateDTO businessTypeUpdateDTO)
    {
        try
        {
            var businessType = await businessTypeService.UpdateBusinessTypeAsync(id, businessTypeUpdateDTO);
            return Ok(businessType);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the business type: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBusinessType(Guid id)
    {
        try
        {
            await businessTypeService.DeleteBusinessTypeAsync(id);
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