using Discount.API.Mapping;
using Discount.API.Services;
using JWTAuthentication.JWT;

namespace Discount.API.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddAuthorization();

            services.AddScoped<IDapperService, DapperService>();
            services.AddScoped<ICouponService, CouponService>();
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
