using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Infrastructure;
using Ordering.Application.Common.Persistence;
using Ordering.Application.Common.Service;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Services;

namespace Ordering.Infrastructure.Extensions
{
    public static class ServiceRegistry
    {
        public static IServiceCollection AddInfrastructureRegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
           configuration.GetSection("Settings").Bind(AppSettings.Settings);
           configuration.GetSection("EmailSettings").Bind(AppSettings.EmailSettings);

           services.AddScoped<IDapperService, DapperService>()
                   .AddScoped<IOrderRepository, OrderRepository>()
                   .AddTransient<IEmailService, EmailService>();

           return services;
        }
    }
}
