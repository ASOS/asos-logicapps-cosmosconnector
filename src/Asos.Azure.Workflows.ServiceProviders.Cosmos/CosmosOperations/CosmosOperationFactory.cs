using System;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations
{
    public class CosmosOperationFactory
    {
        public CosmosOperation GetCosmosOperation(
            string operationId, 
            InsensitiveDictionary<JToken> connectionParameters,
            ServiceOperationRequest serviceOperationRequest)
        {
            return operationId switch
            {
                "upsertDocument" =>  new UpsertOperation(connectionParameters, serviceOperationRequest),
                "queryDocuments" =>  new QueryOperation(connectionParameters, serviceOperationRequest),
                "getDocument" =>  new GetOperation(connectionParameters, serviceOperationRequest),
                _ => throw new InvalidOperationException("Unknown operation specified")
            };
        }
    }
}