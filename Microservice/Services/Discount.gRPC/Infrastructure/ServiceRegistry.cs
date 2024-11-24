using Discount.gRPC.Mapping;
using Discount.gRPC.Services;

namespace Discount.gRPC.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
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
