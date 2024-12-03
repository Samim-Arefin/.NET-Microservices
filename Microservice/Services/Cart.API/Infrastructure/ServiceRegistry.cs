using Cart.API.Services.Implementations;
using Cart.API.Services.Interfaces;
using Discount.gRPC.Protos;
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

            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                 options => options.Address = new Uri(AppSettings.GrpcSettings.DiscountGrpcUri));

            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<IDiscountgRPCService, DiscountgRPCService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

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
