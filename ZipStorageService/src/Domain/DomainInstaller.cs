using Domain.Interfaces.Services;
using Domain.Services;
using Domain.Services.ValidationRules;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DomainInstaller
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddSingleton<IValidationRule, InvalidDllNameValidationRule>();
            services.AddSingleton<IValidationRule, InvalidImagesValidationRule>();
            services.AddSingleton<IValidationRule, InvalidLanguageValidationRule>();

            services.AddSingleton<IZipFileService, ZipFileService>();

            return services;
        }
    }
}
