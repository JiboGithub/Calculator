using Calculator.Infrastructure.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static void AddValidationService<T, TValidator>(this IServiceCollection services)
            where T : class
            where TValidator : class, IValidator<T>
        {
            services.AddScoped<IValidator<T>, TValidator>();
            services.AddScoped<IValidationService<T>, ValidationService<T>>();
        }

        public static void AddValidationServiceFactory(this IServiceCollection services)
        {
            services.AddScoped<IValidationServiceFactory, ValidationServiceFactory>();
        }

        public static void AddValidationExceptionHandler(this IServiceCollection services)
        {
            services.AddScoped<IValidationExceptionHandler, ValidationExceptionHandler>();
        }
    }
}
