using ComingHereClient;
using ComingHereClient.Provider;
using ComingHereClient.Services;
using ComingHereShared.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("StaticFilesClient", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

// Клиент без авторизации
builder.Services.AddHttpClient("PublicClient", client =>
{
    client.BaseAddress = new Uri(ApiUrls.ApiBaseUrl);
});

// Клиент с авторизацией
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri(ApiUrls.ApiBaseUrl);
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));

builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<LocalizationService>();

await builder.Build().RunAsync();