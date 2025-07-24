using Abstraction.Contracts.Repository;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Application.Common.Configurations;
using Minio;
using Infrastructure.Service.Storage;
using Abstraction.Contract.Service.Storage;

namespace Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddTransient(typeof(IGenericRepository), typeof(GenericRepository)).AddSwaggerService();
            services.AddMinIOServices(configuration);
            return services;
        }
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
            .UseSqlServer(configuration.GetConnectionString("default"), builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            //Swagger Gn
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Jadwa Identity API",
                    Version = "v2",
                    Description = "Jadwa Identity API by Jadwa",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Jadwa",
                        Email = "info@Jadwa.com",
                        Url = new Uri("https://linkedin.com/ahmedelbaradey"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Jadwa Identity API LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                s.EnableAnnotations();

                s.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "Jwt Authentication header using the Bearer scheme (....)"
                ,
                    Name = "Authorization"
                ,
                    In = ParameterLocation.Header
                ,
                    Type = SecuritySchemeType.ApiKey
                ,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });


                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                         Type = ReferenceType.SecurityScheme,
                         Id = JwtBearerDefaults.AuthenticationScheme
                         }
                     },
                     Array.Empty<string>()
                   }
                });
            });
            return services;
        }

        public static IServiceCollection AddMinIOServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure MinIO settings
            services.Configure<MinIOConfiguration>(configuration.GetSection(MinIOConfiguration.Section));

            // Register MinIO client
            services.AddSingleton<IMinioClient>(serviceProvider =>
            {
                var config = configuration.GetSection(MinIOConfiguration.Section).Get<MinIOConfiguration>();

                if (config == null || !config.Enabled)
                {
                    // Return a dummy client if MinIO is disabled
                    return new MinioClient()
                        .WithEndpoint("localhost:9000")
                        .WithCredentials("dummy", "dummy")
                        .Build();
                }

                var clientBuilder = new MinioClient()
                    .WithEndpoint(config.Endpoint)
                    .WithCredentials(config.AccessKey, config.SecretKey);

                if (config.UseSSL)
                {
                    clientBuilder = clientBuilder.WithSSL();
                }

                if (!string.IsNullOrEmpty(config.Region))
                {
                    clientBuilder = clientBuilder.WithRegion(config.Region);
                }

                return clientBuilder.Build();
            });

            // Register storage service
            services.AddScoped<IStorageService, StorageService>();

            // Register preview URL helper
            services.AddScoped<IPreviewUrlHelper, PreviewUrlHelper>();

            return services;
        }
    }
}
