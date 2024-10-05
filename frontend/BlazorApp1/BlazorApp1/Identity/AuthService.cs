using BlazorApp1.Identity.Contracts;
using Newtonsoft.Json;

namespace BlazorApp1.Identity
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<AuthResponse?> Login(AuthRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("auth/login", request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthResponse>(content);
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> Register(RegisterRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("auth/register", request);
            return result.IsSuccessStatusCode;
        }
    }
}
