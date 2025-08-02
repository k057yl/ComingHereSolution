using ComingHereClient.Provider;
using ComingHereShared.Constants;
using ComingHereShared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigationManager;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly IJSRuntime _js;

    private string? _token;
    public string? UserEmail { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

    private readonly AuthorizationMessageHandler _authMessageHandler;

    public AuthService(IHttpClientFactory httpClientFactory,
                       NavigationManager navigationManager,
                       CustomAuthStateProvider authStateProvider,
                       IJSRuntime js,
                       AuthorizationMessageHandler authMessageHandler)
    {
        _http = httpClientFactory.CreateClient("AuthorizedClient");
        _navigationManager = navigationManager;
        _authStateProvider = authStateProvider;
        _js = js;
        _authMessageHandler = authMessageHandler;
    }

    public async Task<bool> Login(string email, string password)
    {
        var response = await _http.PostAsJsonAsync(ApiRoutes.Account.Login, new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result != null)
        {
            _token = result.Token;
            UserEmail = result.Email;

            //_http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", _token);

            var roles = ParseRolesFromJwt(_token);
            await _authStateProvider.MarkUserAsAuthenticated(_token, roles);

            return true;
        }
        return false;
    }

    private List<string> ParseRolesFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.Claims
                  .Where(c => c.Type == "role")
                  .Select(c => c.Value)
                  .ToList();
    }

    public async Task Logout()
    {
        _token = null;
        UserEmail = null;

        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");

        _authMessageHandler.ClearTokenCache();

        await _authStateProvider.MarkUserAsLoggedOut();

        _navigationManager.NavigateTo("/", forceLoad: true);
    }
}