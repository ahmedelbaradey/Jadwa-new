using Application.Common.Behaviors;
using Application.Common.Configurations;
using Application.Features.Notification;
using Application.Mapping.Resolutions;
using Application.Mapping.Users;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Application
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplicationModuleDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(o => o.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.Configure<AttachmentConfiguration>(configuration.GetSection(AttachmentConfiguration.Section));
            services.Configure<MinIOConfiguration>(configuration.GetSection(MinIOConfiguration.Section));
            services.AddHostedService<FundNotificationJob>();

            // Register AutoMapper custom value resolvers for BoardMember display properties
            //services.AddScoped<BoardMemberTypeDisplayResolver>();
            //services.AddScoped<BoardMemberStatusDisplayResolver>();
            //services.AddScoped<BoardMemberRoleDisplayResolver>();

            // Register AutoMapper custom value resolvers for Resolution display properties
            services.AddScoped<ResolutionStatusDisplayResolver>();

 
            services.AddScoped<VotingTypeDisplayResolver>();
            services.AddScoped<MemberVotingResultDisplayResolver>();


            return services;
        }
    }
}
