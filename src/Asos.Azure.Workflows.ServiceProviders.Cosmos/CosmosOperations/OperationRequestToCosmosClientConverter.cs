using System.Linq;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Configuration;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Infrastructure;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations
{
    public class OperationRequestToCosmosClientConverter
    {
        private readonly ServiceOperationRequest _operationRequest;
        private readonly InsensitiveDictionary<JToken> _connectionParameters;

        public OperationRequestToCosmosClientConverter(ServiceOperationRequest operationRequest,
            InsensitiveDictionary<JToken> connectionParameters)
        {
            _operationRequest = operationRequest;
            _connectionParameters = connectionParameters;
        }
        
        public CosmosDbClient GetCosmosDbClient()
        {
            var connectionString = _connectionParameters.Values.ToList()[0];
            var databaseName = _operationRequest.Parameters["databaseName"].ToString();
            var containerId = _operationRequest.Parameters["containerId"].ToString();

            var config = new DocumentDbConfiguration(connectionString.ToString(), databaseName, containerId);
            return new CosmosDbClient(config);
        }
    }
}