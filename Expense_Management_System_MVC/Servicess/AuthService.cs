using Expense_Management_System_MVC.DTO.Auth;
using Expense_Management_System_MVC.Services.Interfaces;
using System.Net.Http.Json;

namespace Expense_Management_System_MVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            _httpClient.BaseAddress = new Uri(
                _configuration["ApiSettings:BaseUrl"]!
            );

            var response = await _httpClient.PostAsJsonAsync(
                "api/Auth/login",
                request
            );

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
    }
}