using ComingHereClient;
using ComingHereClient.Provider;
using ComingHereClient.Services;
using ComingHereShared.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient для статики (локализация)
builder.Services.AddHttpClient("StaticFilesClient", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

// Клиент без авторизации
builder.Services.AddHttpClient("PublicClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

// Клиент с авторизацией
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
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

var host = builder.Build();

try
{
    var js = host.Services.GetRequiredService<IJSRuntime>();
    var culture = await js.InvokeAsync<string>("localStorage.getItem", "blazorCulture") ?? "uk";

    var loc = host.Services.GetRequiredService<LocalizationService>();
    await loc.LoadAsync(culture);
}
catch (Exception ex)
{
    Console.WriteLine($"Localization load failed: {ex.Message}");
}

await host.RunAsync();