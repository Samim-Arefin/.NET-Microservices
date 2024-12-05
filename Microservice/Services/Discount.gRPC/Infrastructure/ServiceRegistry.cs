using Discount.gRPC.Mapping;
using Discount.gRPC.Services;
using JWTAuthentication.JWT;

namespace Discount.gRPC.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddAuthorization();

            services.AddGrpc(options => 
            {
                options.Interceptors.Add<ExceptionInterceptor>();
            });
            services.AddScoped<IDapperService, DapperService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddAutoMapper(typeof(MapperProfile));
            return services;
        }
    }
}
