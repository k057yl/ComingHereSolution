using Microsoft.JSInterop;
using System.Net.Http.Headers;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;
    private string? _cachedToken;

    public AuthorizationMessageHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void ClearTokenCache() => _cachedToken = null;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_cachedToken))
        {
            _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }
        if (!string.IsNullOrEmpty(_cachedToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}