using ComingHereServer.Data.Interfaces;
using ComingHereServer.Data.Repositories;
using ComingHereServer.Data.UnitOfWorks;

namespace ComingHereServer.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
