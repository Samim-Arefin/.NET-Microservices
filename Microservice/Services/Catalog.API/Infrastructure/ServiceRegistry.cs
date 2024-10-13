using Catalog.API.Data;
using Catalog.API.Mapping;
using Catalog.API.Services.Implementations;
using Catalog.API.Services.Interfaces;

namespace Catalog.API.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IProductService, ProductService>();
            services.AddAutoMapper(typeof(MapperProfile));
            return services;
        }
    }
}
