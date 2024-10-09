using BlazorApp1.Identity.Services;
using Intersoft.Crosslight.Mobile;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using ILocalStorageService = Blazored.LocalStorage.ILocalStorageService;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("12345");

        if (string.IsNullOrWhiteSpace(token))
        {
            // No token means the user is not authenticated
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // If token exists, create a ClaimsPrincipal with the appropriate claims
        var claims = JwtParser.ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwtAuth");
        var user = new ClaimsPrincipal(identity);

        // Set the default authorization header for future API calls
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return new AuthenticationState(user);
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var claims = JwtParser.ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwtAuth");
        var user = new ClaimsPrincipal(identity);

        // Notify Blazor that the authentication state has changed
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        // Clear the authorization header
        _httpClient.DefaultRequestHeaders.Authorization = null;

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}

