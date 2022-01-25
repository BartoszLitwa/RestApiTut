﻿using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetBook.Domain;

namespace TweetBook.Installers
{
    public class CosmosInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var cosmosStoreSettings = new CosmosStoreSettings(
                databaseName: configuration["CosmosSettings:DatabaseName"],
                endpointUrl: configuration["CosmosSettings:AccountUri"],
                authKey: configuration["CosmosSettings:AccountKey"],
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp});

            services.AddCosmosStore<CosmosPostDto>(cosmosStoreSettings);
        }
    }
}
