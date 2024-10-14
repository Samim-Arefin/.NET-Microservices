using Cart.API.Services.Implementations;
using Cart.API.Services.Interfaces;
using StackExchange.Redis;

namespace Cart.API.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = AppSettings.Settings.ConnectionStrings;
                options.ConfigurationOptions = new ConfigurationOptions()
                {
                    AbortOnConnectFail = true,
                    EndPoints = { options.Configuration }
                };
            });

            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }
    }
}
