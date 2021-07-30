using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Json;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations
{
    public class GetOperation : CosmosOperation
    {
        public GetOperation(
            InsensitiveDictionary<JToken> connectionParameters, 
            ServiceOperationRequest serviceOperationRequest) 
            : base(connectionParameters, serviceOperationRequest)
        {
        }

        protected override async Task<ServiceOperationResponse> DoInvocation()
        {
            var cosmosDbClient = 
                new OperationRequestToCosmosClientConverter(ServiceOperationRequest, ConnectionParameters)
                    .GetCosmosDbClient();
            
            var documentId = ServiceOperationRequest.Parameters["documentId"].ToString();
            var partitionKey = ServiceOperationRequest.Parameters["partitionKey"].ToString();

            ItemResponse<object> result;
            
            try
            {
                result = await cosmosDbClient.GetDocumentAsync<object>(documentId, partitionKey);
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new ServiceOperationResponse(new
                    {
                        Code = HttpStatusCode.NotFound.ToString(), e.Message
                    }.ToJToken());
            }
            
            var responsePayload = JsonSerializer.Serialize(result.Resource);
            return new ServiceOperationResponse(JObject.Parse(responsePayload), result.StatusCode);
        }
    }
}