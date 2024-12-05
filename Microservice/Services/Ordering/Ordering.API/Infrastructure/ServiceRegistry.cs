using JWTAuthentication.JWT;
using MassTransit;
using Ordering.API.EventBusHandler;
using Ordering.API.Mapping;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication();
            services.AddAuthorization();

            services.AddApplicationServices();
            services.AddInfrastructureRegisterServices(configuration);
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<CartCheckOutEventHandler>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Settings.MessageBrokerSettings.Host);

                    cfg.ConfigureEndpoints(context);
                });
            });

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
