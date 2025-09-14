using ComingHereShared.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromMemory(
        new[]
        {
            new Yarp.ReverseProxy.Configuration.RouteConfig
            {
                RouteId = "api",
                Match = new() { Path = "/api/{**catch-all}" },
                ClusterId = "main-server"
            }
        },
        new[]
        {
            new Yarp.ReverseProxy.Configuration.ClusterConfig
            {
                ClusterId = "main-server",
                Destinations = new Dictionary<string, Yarp.ReverseProxy.Configuration.DestinationConfig>
                {
                    { "d1", new() { Address = ApiUrls.SERVER_URL} }
                }
            }
        }
    );

var app = builder.Build();
app.UseRouting();
app.MapReverseProxy();
app.Run();