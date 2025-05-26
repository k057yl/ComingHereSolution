using ComingHereShared.DTO;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ComingHereClient.Provider
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;
        private string? _token;
        public string? UserEmail { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

        public AuthService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/account/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (result != null)
            {
                _token = result.Token;
                UserEmail = result.Email;
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                return true;
            }
            return false;
        }

        public void Logout()
        {
            _token = null;
            UserEmail = null;
            _http.DefaultRequestHeaders.Authorization = null;

            _navigationManager.NavigateTo("/");
        }
    }
}
