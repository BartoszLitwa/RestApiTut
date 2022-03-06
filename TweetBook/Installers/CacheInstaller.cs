using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TweetBook.Cache;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if(!redisCacheSettings.Enabled)
            {
                return;
            }

            // We have to register it for redis health check to work
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisCacheSettings.ConnectionString;
            });

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
