using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Infrastructure
{
    public static class CosmosClientInstance
    {
        private static readonly ConcurrentDictionary<string, CosmosClient> CosmosClients = new ConcurrentDictionary<string, CosmosClient>();
        
        public static CosmosClient GetCosmosClient(string connectionString)
        {
            var connectionStringDetails = new CosmosDbConnectionStringDetails(connectionString);
            var clientCacheKey = connectionStringDetails.ServiceEndpoint;
                            
            if (CosmosClients.TryGetValue(clientCacheKey, out var client))
            {
                return client;
            }
            
            var cosmosClient = 
                new CosmosClientBuilder(connectionString)
                    .WithCustomSerializer(
                        new SystemTextJsonSerializer(new JsonSerializerOptions()))
                    .Build();

            CosmosClients.TryAdd(clientCacheKey, cosmosClient);
            return cosmosClient;
        }
    }
}