using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComingHereClient;
using ComingHereClient.Provider;
using ComingHereClient.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("PublicClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7255/");
});

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7255/");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();