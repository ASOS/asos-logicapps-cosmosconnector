using System.Text.Json.Serialization;
using Asos.Azure.Workflows.ServiceProviders.Cosmos;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Models
{
    public class TestDocument : CosmosDocument
    {
        [JsonPropertyName("field1")] 
        public string Field1 { get; set; }

        [JsonPropertyName("field2")]
        public string Field2 { get; set; }

        [JsonPropertyName("ttl")]
        public long Ttl { get; set; }
    }
}