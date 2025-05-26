using ComingHereServer.Services;

namespace ComingHereServer
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<EmailService>();
            services.AddTransient<ConfirmationCodeGenerator>();

            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            
        }
    }
}
