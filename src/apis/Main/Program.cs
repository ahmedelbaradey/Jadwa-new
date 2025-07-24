using Extensions;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using Application.Common.MiddleWare;
using Presentation.Middleware;
using Infrastructure;
using Main.Extensions;
using Infrastructure.Data.Config;
using Presentation.Bases;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Abstraction.Contract.Service;
using Presentation.Controllers.Identity;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");// ?? "Production";
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
     .AddJsonFile("systemConfiguration.json")
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .Build();

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


builder.Services.ConfigureInfrastructureDependencies(configuration);
builder.Services.ConfigureIdentityService(configuration);
builder.Services.ConfigureCatalogServices(configuration);
builder.Services.AddLoggerServices(configuration);
builder.Services.ConfigureRateLimitingOptions();
builder.Services.ConfigureCors(configuration);
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.AddMemoryCache();
builder.Services.ConfigureLocalization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jadwa-6a040-firebase-adminsdk-fbsvc-679bd6d6d4.json")),
});

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() => new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson().Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();
builder.Services.AddValidatorsFromAssembly(typeof(Infrastructure.AssemblyReference).Assembly);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.InvalidModelStateResponseFactory = ValidationErrorResponseFactory.CreateResponse;
});
builder.Services.AddControllers(config =>
{
     //config.Filters.Add<ValidationMiddleware>();
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
}).AddXmlDataContractSerializerFormatters().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

 
var app = builder.Build();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Jadwa API v2");
        options.DisplayRequestDuration();
    });

var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

app.UseHsts();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await Seed.SeedAsync(services);
}

//Handle Error Middleware
app.UseMiddleware<ErrorHandlerMiddleWare>();
app.UseMiddleware<AuthorizationLoggingMiddleware>();
app.UseIpRateLimiting();
app.UseResponseCaching();
app.UseAuthentication();
//app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
