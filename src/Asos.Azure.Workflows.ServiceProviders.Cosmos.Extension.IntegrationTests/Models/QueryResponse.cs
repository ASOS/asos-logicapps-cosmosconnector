using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Models
{
    public class QueryResponse<T>
    {
        [JsonPropertyName("value")] 
        public IList<T> Documents { get; set; }

        [JsonPropertyName("continuationToken")] 
        public string ContinuationToken { get; set; }       
    }
}