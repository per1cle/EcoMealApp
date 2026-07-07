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
}