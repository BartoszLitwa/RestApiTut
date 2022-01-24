using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace TweetBook.Installers
{
    public static class InstallerExtensions
    {
        public static IServiceCollection InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes
                // Selecting All Installers
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                // Make an instance of each
                .Select(Activator.CreateInstance)
                // Cast each instance to IInstaller
                .Cast<IInstaller>()
                .ToList();

            // For each call Install Services
            installers.ForEach(x => x.InstallServices(services, configuration));

            return services;
        }
    }
}
