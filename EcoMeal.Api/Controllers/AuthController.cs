using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcoMeal.DataAccess.Entities;
using EcoMeal.Shared.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EcoMeal.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);
        if (userExists != null)
            return BadRequest(new AuthResponseDTO { IsSuccess = false, ErrorMessage = "User already exists!" });

        var user = new User()
        {
            Email = registerDTO.Email,
            UserName = registerDTO.Email,
            Name = registerDTO.Name
        };
        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new AuthResponseDTO { IsSuccess = false, ErrorMessage = errors });
        }

        if (!await _roleManager.RoleExistsAsync("Customer"))
            await _roleManager.CreateAsync(new Role { Name = "Customer" });
        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new Role { Name = "Admin" });
        if (!await _roleManager.RoleExistsAsync("Business"))
            await _roleManager.CreateAsync(new Role { Name = "Business" });

        await _userManager.AddToRoleAsync(user, "Customer");

        return Ok(new AuthResponseDTO { IsSuccess = true });

    }

    [HttpPost("register-business")]
    public async Task<IActionResult> RegisterBusiness([FromBody] RegisterDTO registerDTO)
    {
        var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);
        if (userExists != null)
        {
            return BadRequest(new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Email is already in use!" });
        }

        var user = new User
        {
            Email = registerDTO.Email,
            UserName = registerDTO.Email,
            Name = registerDTO.Name
        };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new AuthResponseDTO { IsSuccess = false, ErrorMessage = errors });
        }

        if (!await _roleManager.RoleExistsAsync("Business"))
            await _roleManager.CreateAsync(new Role { Name = "Business" });

        await _userManager.AddToRoleAsync(user, "Business");

        return Ok(new AuthResponseDTO { IsSuccess = true });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            return Unauthorized(new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Email or password is invalid." });
        }

        var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = GenerateJwtToken(authClaims);

        return Ok(new AuthResponseDTO
        {
            IsSuccess = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("Secret Key for JWT is not configured in appsettings.json!");
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
