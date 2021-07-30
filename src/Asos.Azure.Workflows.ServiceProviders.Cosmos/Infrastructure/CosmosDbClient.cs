using System.Text.Json;
using System.Threading.Tasks;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Configuration;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations;
using Microsoft.Azure.Cosmos;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Infrastructure
{
    public class CosmosDbClient
    {
        public DocumentDbConfiguration DbConfiguration { get; }

        public CosmosDbClient(DocumentDbConfiguration configuration)
        {
            DbConfiguration = configuration;
        }

        public Task<ItemResponse<T>> GetDocumentAsync<T>(string documentId, string partitionKey)
        {
            var container = GetCosmosContainer();

            return container.ReadItemAsync<T>(documentId, new PartitionKey(partitionKey));
        }

        public Task<ItemResponse<object>> UpsertDocumentAsync(string json)
        {
            var container = GetCosmosContainer();

            var objectToPersist = JsonSerializer.Deserialize<object>(json);

            return container.UpsertItemAsync(objectToPersist);
        }

        public async Task<FeedResponse<T>> QueryItemsAsync<T>(QueryParameters queryParameters)
        {
            var container = GetCosmosContainer();

            var queryDefinition = new QueryDefinition(queryParameters.SqlQuery);

            var queryResultSetIterator = container.GetItemQueryIterator<T>(
                queryDefinition,
                string.IsNullOrEmpty(queryParameters.ContinuationToken) ? null : queryParameters.ContinuationToken,
                new QueryRequestOptions
                {
                    MaxItemCount = queryParameters.MaximumItemCount
                });

            var resultSet = await queryResultSetIterator.ReadNextAsync();

            return resultSet;
        }

        private Container GetCosmosContainer()
        {
            var client = CosmosClientInstance.GetCosmosClient(DbConfiguration.ConnectionString);
            
            var database = client.GetDatabase(DbConfiguration.DatabaseName);
            var container = database.GetContainer(DbConfiguration.ContainerId);

            return container;
        }
    }
}