using System.Net.Http.Json;
using Blazored.LocalStorage;
using EcoMeal.Shared.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Components.Authorization;
using EcoMeal.UI.Providers;

namespace EcoMeal.UI.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<AuthResponseDTO> Login(LoginDTO loginModel)
    {
        // 1. Apelăm API-ul (AuthController.cs)
        var response = await _http.PostAsJsonAsync("api/auth/login", loginModel);
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

        if (response.IsSuccessStatusCode && result != null && result.IsSuccess)
        {
            // 2. Salvăm Token-ul în browser
            await _localStorage.SetItemAsync("authToken", result.Token);

            // 3. Anunțăm CustomProvider-ul că utilizatorul s-a conectat cu succes
            ((CustomAuthStateProvider)_authStateProvider).MarkUserAsAuthenticated(result.Token!);

            // 4. Punem token-ul pe HttpClient ca să meargă și cererile viitoare (ex: Edit Business)
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

            return result;
        }

        return result ?? new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Unknown login error." };
    }

    public async Task<AuthResponseDTO> Register(RegisterDTO registerModel)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", registerModel);
        return await response.Content.ReadFromJsonAsync<AuthResponseDTO>()
               ?? new AuthResponseDTO { IsSuccess = false, ErrorMessage = "API did not return a valid response." };
    }

    public async Task<AuthResponseDTO> RegisterBusiness(RegisterDTO registerModel)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register-business", registerModel);
        return await response.Content.ReadFromJsonAsync<AuthResponseDTO>()
               ?? new AuthResponseDTO { IsSuccess = false, ErrorMessage = "API did not return a valid response." };
    }

    public async Task Logout()
    {
        // Ștergem token-ul și resetăm starea
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthStateProvider)_authStateProvider).MarkUserAsLoggedOut();
        _http.DefaultRequestHeaders.Authorization = null;
    }
}