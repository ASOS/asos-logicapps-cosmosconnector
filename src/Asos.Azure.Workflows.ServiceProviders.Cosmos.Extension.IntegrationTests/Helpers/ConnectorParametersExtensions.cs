using Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Helpers
{
    public static class ConnectorParametersExtensions
    {
        public static void AssignQueryParameters(
            this InsensitiveDictionary<JToken> connectorParameters, 
            QueryParameters queryParameters)
        {
            connectorParameters.Add("sqlQuery", queryParameters.SqlQuery);
            connectorParameters.Add("maximumItemCount", queryParameters.MaximumItemCount);
            connectorParameters.Add("continuationToken", queryParameters.ContinuationToken);
        }
    }
}
