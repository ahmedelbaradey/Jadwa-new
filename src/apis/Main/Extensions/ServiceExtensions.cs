using Asp.Versioning;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Infrastructure;
using Application;

namespace Extensions
{
    public static class ServiceExtensions
    {
        
        public static void ConfigureInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureDependencies(configuration);
        }
        public static void ConfigureIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityService(configuration).ConfigureRedisService(configuration);
        }
        public static void ConfigureRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            string redisInstanceName = configuration.GetValue<string>("RedisInstanceName");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisInstanceName;
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    AbortOnConnectFail = true,
                    EndPoints = { options.Configuration }
                };
            });
        }  
        public static void ConfigureCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationModuleDependencies(configuration).AddDataServices();
        }
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration) => services.AddCors(options => 
        {
            string urls = configuration.GetValue<string>("AllowedOrigins");
            options.AddPolicy("CorsPolicy", builder =>
            builder.WithOrigins(urls)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
            .WithExposedHeaders("X-Pagination"));
        });      
        public static void ConfigureIISIntegration(this IServiceCollection services) => services.Configure<IISOptions>(options =>{});
        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule> { new RateLimitRule { Endpoint = "*", Limit = 200, Period = "1m" } };
            services.Configure<IpRateLimitOptions>(opt => { opt.GeneralRules = rateLimitRules; });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
        public static void ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "";
            });

            services.Configure<RequestLocalizationOptions>(opt =>
            {
                List<CultureInfo> locales = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("ar-EG"), };
                opt.DefaultRequestCulture = new RequestCulture("ar-EG");
                opt.SupportedCultures = locales;
                opt.SupportedUICultures = locales;
            });
        }
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(2, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            }).EnableApiVersionBinding().AddMvc();
        }
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) => services.AddHttpCacheHeaders((expirationOpt) => { expirationOpt.MaxAge = 65; expirationOpt.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private; }, (validationOpt) => { validationOpt.MustRevalidate = true; });
        public static void ConfigureForwardedHeaders(this IServiceCollection services) => services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
    }
}
