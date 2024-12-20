﻿using Cart.API.Mapping;
using Cart.API.Services.Implementations;
using Cart.API.Services.Interfaces;
using Discount.gRPC.Protos;
using Grpc.Core;
using JWTAuthentication.JWT;
using MassTransit;
using StackExchange.Redis;

namespace Cart.API.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddAuthorization();
            services.AddHttpContextAccessor();

            services.AddScoped<IRedisCacheService, RedisCacheService>()
                    .AddScoped<IDiscountgRPCService, DiscountgRPCService>()
                    .AddScoped<ICartCheckOutService, CartCheckOutService>()
                    .AddScoped<ITokenProviderService, TokenProviderService>()
                    .AddTransient<IEventBusService, EventBusService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = AppSettings.Settings.ConnectionStrings;
                options.ConfigurationOptions = new ConfigurationOptions()
                {
                    AbortOnConnectFail = true,
                    EndPoints = { options.Configuration }
                };
            });

            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
            {
                options.Address = new Uri(AppSettings.GrpcSettings.DiscountGrpcUri);
            })
            .AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var provider = serviceProvider.GetRequiredService<ITokenProviderService>();
                var token = await provider.GetTokenAsync();
                metadata.Add("Authorization", $"Bearer {token}");
            })
            .ConfigureChannel(options =>
            {
                options.UnsafeUseInsecureChannelCallCredentials = true;
            });


            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(AppSettings.MessageBrokerSettings.Host);

                    cfg.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(5)));

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
