using System;
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
    public class UpsertOperation : CosmosOperation
    {
        public UpsertOperation(
            InsensitiveDictionary<JToken> connectionParameters, 
            ServiceOperationRequest serviceOperationRequest) 
            : base(connectionParameters, serviceOperationRequest)
        {
        }

        protected override async Task<ServiceOperationResponse> DoInvocation()
        {
            var payload = ServiceOperationRequest.Parameters["jsonDocument"].ToString();
            if (string.IsNullOrEmpty(payload) || payload == "{}")
            {
                return new ServiceOperationResponse(JObject.Parse("{}"), HttpStatusCode.NoContent);
            }
            
            var cosmosDbClient = 
                new OperationRequestToCosmosClientConverter(ServiceOperationRequest, ConnectionParameters)
                .GetCosmosDbClient();

            try
            {
                var result =
                    await cosmosDbClient.UpsertDocumentAsync(ServiceOperationRequest.Parameters["jsonDocument"]
                        .ToString());

                var upsertedDocument = JsonSerializer.Serialize(result.Resource);

                return new ServiceOperationResponse(JObject.Parse(upsertedDocument), result.StatusCode);
            }
            catch (JsonException e)
            {
                return ReturnBadRequestResponse(e);
            }
            catch (CosmosException e)
            {
                return ReturnBadRequestResponse(e);
            }
        }
    }
}