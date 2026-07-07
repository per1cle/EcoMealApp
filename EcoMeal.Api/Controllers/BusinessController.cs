using Microsoft.AspNetCore.Mvc;
using EcoMeal.Shared.DTOs.BusinessDTOs;
using EcoMeal.BusinessLogic.Services.Interfaces;
namespace EcoMeal.Api.Controllers;

[ApiController]
[Route("api/businesses")]
public class BusinessController(IBusinessService businessService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<BusinessGetDTO>>> GetBusinesses()
    {
        try
        {
            var businesses = await businessService.GetAllBusinessesAsync();
            return Ok(businesses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving businesses: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<BusinessGetDTO>> AddBusiness(BusinessCreateDTO businessCreateDTO)
    {
        try
        {
            var business = await businessService.AddBusinessAsync(businessCreateDTO);
            return CreatedAtAction(nameof(GetBusinesses), new { id = business.Id }, business);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the business: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BusinessGetDTO>> UpdateBusiness(Guid id, BusinessUpdateDTO businessUpdateDTO)
    {
        try
        {
            var business = await businessService.UpdateBusinessAsync(id, businessUpdateDTO);
            return Ok(business);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the business: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBusiness(Guid id)
    {
        try
        {
            await businessService.DeleteBusinessAsync(id);
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