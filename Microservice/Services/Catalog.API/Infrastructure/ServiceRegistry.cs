using Catalog.API.Data;
using Catalog.API.Mapping;
using Catalog.API.Services.Implementations;
using Catalog.API.Services.Interfaces;
using JWTAuthentication.JWT;

namespace Catalog.API.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddAuthorization();

            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IProductService, ProductService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddAutoMapper(typeof(MapperProfile));
            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy",
                  policy =>
                  {
                      policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
                  });
            });
            return services;
        }
    }
}
