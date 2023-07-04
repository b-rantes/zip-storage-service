using Domain.Interfaces.Repository;
using Infrastructure.Repositories.ZipFileRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IZipFileRepository, InMemoryZipFileRepository>();

            return services;
        }
    }
}
