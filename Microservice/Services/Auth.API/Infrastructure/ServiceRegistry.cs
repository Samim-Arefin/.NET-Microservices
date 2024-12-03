using Auth.API.Entities;
using Auth.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.API.Enums;
using System.Text;

namespace Auth.API.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                     {
                          options.UseSqlServer(AppSettings.Settings.ConnectionStrings);
                     })
                    .AddIdentity<AppUser, AppRole>(options =>
                    {
                        options.Password.RequireDigit = true;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = true;
                        options.User.RequireUniqueEmail = true;
                        options.Lockout.AllowedForNewUsers = true;
                        options.Lockout.MaxFailedAccessAttempts = 3;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                        options.Tokens.PasswordResetTokenProvider = "resetPassword";
                    }) 
                        .AddDefaultTokenProviders()
                        .AddTokenProvider<ResetPasswordEmailTokenProvider<AppUser>>("resetPassword")
                        .AddEntityFrameworkStores<AppDbContext>();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(12);
            })
            .Configure<ResetPasswordEmailTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(option =>
               {
                   option.SaveToken = true;
                   option.RequireHttpsMetadata = false;
                   option.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidIssuer = AppSettings.Settings.ApiUri,
                       ValidateAudience = false,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Settings.TokenKey))
                   };
               });

            services.AddAuthorizationCore();

            services.AddScoped<IAuthenticationService, AuthenticationService>()
                    .AddTransient<IEmailService, EmailService>();

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

        public static void ApplyDatabaseMigrations(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (dbContext.Database.GetMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
            if (!dbContext.Users.Any())
            {
                var adminRoleId = Guid.NewGuid();
                var adminUser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = AppSettings.Settings.AdminEmail.Split('@')[0],
                    Email = AppSettings.Settings.AdminEmail,
                    NormalizedEmail = AppSettings.Settings.AdminEmail.ToUpper(),
                    NormalizedUserName = AppSettings.Settings.AdminEmail.ToUpper(),
                    EmailConfirmed = true,
                };

                var passwordHasher = new PasswordHasher<AppUser>();
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, AppSettings.Settings.AdminPassword);
                dbContext.Users.Add(adminUser);

                dbContext.Roles.AddRange(
                  new List<AppRole> {
                        new()
                        {
                            Id = adminRoleId,
                            Name = UserRole.Admin.ToString(),
                            NormalizedName = UserRole.Admin.ToString().ToUpper(),
                        },
                        new()
                        {
                            Name = UserRole.User.ToString(),
                            NormalizedName = UserRole.User.ToString().ToUpper()
                        }
                  });

                dbContext.UserRoles.Add(
                   new AppUserRole
                   {
                       UserId = adminUser.Id,
                       RoleId = adminRoleId
                   });

                dbContext.SaveChanges();
            }
        }
    }
}
