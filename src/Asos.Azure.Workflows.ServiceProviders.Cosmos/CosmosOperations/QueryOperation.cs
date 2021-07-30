using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations
{
    public class QueryOperation : CosmosOperation
    {
        public QueryOperation(
            InsensitiveDictionary<JToken> connectionParameters,
            ServiceOperationRequest serviceOperationRequest)
            : base(connectionParameters, serviceOperationRequest)
        {
        }

        protected override async Task<ServiceOperationResponse> DoInvocation()
        {
            var queryParameters = new QueryParameters
            {
                SqlQuery = ServiceOperationRequest.Parameters["sqlQuery"].ToString(),
                MaximumItemCount = ServiceOperationRequest.Parameters["maximumItemCount"].ToObject<int?>(),
                ContinuationToken = ServiceOperationRequest.Parameters["continuationToken"]?.ToString()
            };

            var cosmosDbClient =
                new OperationRequestToCosmosClientConverter(ServiceOperationRequest, ConnectionParameters)
                    .GetCosmosDbClient();

            try
            {
                var result = await cosmosDbClient.QueryItemsAsync<object>(queryParameters);

                var responsePayload = JsonSerializer.Serialize(result.Resource);

                return new ServiceOperationResponse(new JObject
                {
                    { "continuationToken", result.ContinuationToken },
                    { "value",  JArray.Parse(responsePayload) }
                }, result.StatusCode);

            }
            catch (CosmosException e)
            {
                return ReturnBadRequestResponse(e);
            }
        }
    }
}