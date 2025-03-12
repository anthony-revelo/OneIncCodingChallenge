using FluentValidation;
using FluentValidation.AspNetCore;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Validation;

namespace UserManagement.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseClient, DatabaseClient>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void ConfigureValidators(this IServiceCollection services)
        {
 
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<UserValidator>();

        }
    }
}
