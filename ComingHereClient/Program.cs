using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComingHereClient;
using ComingHereClient.Provider;
using ComingHereClient.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

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
    client.BaseAddress = new Uri("https://localhost:7255/");
});

// Клиент с авторизацией
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7255/");
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