using Abstraction.Constants;
using System.Text;
using Abstraction.Contract.Service.Notifications;
using Core.Abstraction.Contract.Service.Notifications;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Abstraction.Contracts.Service;
using Domain.Entities.Users;
using Domain.Helpers;
using Infrastructure.Data;
using Infrastructure.Logger;
using Infrastructure.Repository;
using Infrastructure.Service;
using Infrastructure.Service.Audit;
using Infrastructure.Service.Notifications;
using Infrastructure.Service.Sessions;
using Abstraction.Contract.Service.Sessions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using FluentValidation;
using Domain.Services.Audit;
 


namespace Infrastructure
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IServiceManager, ServiceManager>();
            //Onion Services
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.FullName.Contains("Onion") && t.IsClass && !t.IsAbstract && IsSubclassOfRawGeneric(t, typeof(BaseService<>)));
            foreach (var implementationType in types)
            {
                var interfaceTypes = implementationType.GetInterfaces().Where(i => !i.Name.StartsWith("IBase") && i.Assembly == typeof(Abstraction.AssemblyReference).Assembly);
                foreach (var interfaceType in interfaceTypes)
                {
                    if (!services.Any(s => s.ServiceType == interfaceType))
                    {
                        services.AddScoped(interfaceType, implementationType);
                    }
                }
            }
            services.AddTransient<IIdentityServiceManager, IdentityServiceManager>();
            services.AddScoped<IRepositoryManager, ProductRepositoryManager>();
            services.AddScoped<INotificationLocalizationService, NotificationLocalizationService>();
            services.AddScoped<IWhatsAppNotificationService, WhatsAppNotificationService>();
            services.AddHttpClient<WhatsAppNotificationService>();
            services.AddScoped<IAuditLocalizationService, AuditLocalizationService>();
            services.AddScoped<ISessionManagementService, SessionManagementService>();

            return services;
        }
        public static IServiceCollection AddLoggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            return services;
        }
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            //this is the first code for check the username and password
            services.AddIdentity<User, Role>(opt =>
            {
                //Some Options for Loggin And Password & etc....

                // Password settings.
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.AllowedForNewUsers = true;

                // User settings.
                opt.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            //Binding between jwtSettings Json & JwtSettings Class
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            //Binding between sessionSettings Json & SessionSettings Class
            var sessionSettings = new SessionSettings();
            configuration.GetSection(nameof(sessionSettings)).Bind(sessionSettings);
            services.AddSingleton(sessionSettings);

            //Jwt Authentication settings 
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssure,
                    ValidIssuers = new[] { jwtSettings.Issure },
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssureSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateAudience = jwtSettings.validateAudience,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime
                };
            });

            services.AddAuthorization(options =>
            {
                foreach (var module in Claims.GenerateModules())
                {
                    foreach (var permission in Claims.GeneratePermissions(module))
                    {
                       options.AddPolicy(permission, policy => { policy.RequireClaim(CustomClaimTypes.Permission, permission); });
                    }
                }
            });
            return services;
        }
        private static bool IsSubclassOfRawGeneric(Type type, Type generic)
        {
            while (type != null && type != typeof(object))
            {
                var current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (current == generic)
                    return true;
                type = type.BaseType;
            }
            return false;
        }
    }
}
