using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests
{
    public class ConnectorParameters
    {
        private const string CosmosConnectionString =
            "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public InsensitiveDictionary<JToken> ConnectionParameters { get; } = new InsensitiveDictionary<JToken>();
        public InsensitiveDictionary<JToken> Parameters { get; } = new InsensitiveDictionary<JToken>();

        public ConnectorParameters()
        {
            ConnectionParameters.Add("connectionString", CosmosConnectionString);
            Parameters.Add("databaseName", "ConnectorTesting");
            Parameters.Add("containerId", "Items");
        }
    }
}