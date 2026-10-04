using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EcoMeal.DataAccess.Entities;
using EcoMeal.Shared.DTOs.UserDTOs;

namespace EcoMeal.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UserController(UserManager<User> userManager, RoleManager<Role> roleManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserGetDTO>>> GetUsers()
    {
        try
        {
            var users = await userManager.Users.ToListAsync();
            var result = new List<UserGetDTO>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                result.Add(new UserGetDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "Customer"
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving users: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserGetDTO>> GetUser(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound($"User with ID {id} not found.");

        var roles = await userManager.GetRolesAsync(user);
        return Ok(new UserGetDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? "Customer"
        });
    }

    [HttpPost]
    public async Task<ActionResult<UserGetDTO>> CreateUser([FromBody] UserCreateDTO dto)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return BadRequest("A user with this email already exists.");
            }

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            if (!await roleManager.RoleExistsAsync(dto.Role))
            {
                await roleManager.CreateAsync(new Role { Name = dto.Role });
            }

            await userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new UserGetDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = dto.Role
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating user: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserGetDTO>> UpdateUser(Guid id, [FromBody] UserUpdateDTO dto)
    {
        try
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound($"User with ID {id} not found.");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.UserName = dto.Email;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            // Update role if changed
            var currentRoles = await userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(dto.Role))
            {
                if (currentRoles.Any())
                {
                    await userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                if (!await roleManager.RoleExistsAsync(dto.Role))
                {
                    await roleManager.CreateAsync(new Role { Name = dto.Role });
                }

                await userManager.AddToRoleAsync(user, dto.Role);
            }

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await userManager.ResetPasswordAsync(user, token, dto.NewPassword);
                if (!passResult.Succeeded)
                {
                    var errors = string.Join(", ", passResult.Errors.Select(e => e.Description));
                    return BadRequest(errors);
                }
            }

            return Ok(new UserGetDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                Role = dto.Role
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating user: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        try
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound($"User with ID {id} not found.");

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting user: {ex.Message}");
        }
    }
}
