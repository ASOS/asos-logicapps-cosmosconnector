using System.Collections.Generic;
using System.Threading.Tasks;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Triggers;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Extensions;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos
{
    [ServiceOperationsProvider(Id = ServiceId, Name = ServiceName)]
    public class CosmosServiceOperationsProvider : IServiceOperationsProvider
    {
        public const string ServiceName = "cosmos";

        public const string ServiceId = "/serviceProviders/cosmos";

        private readonly List<ServiceOperation> _serviceOperationsList;
        private readonly InsensitiveDictionary<ServiceOperation> _apiOperationsList;
        private readonly CosmosOperationFactory _cosmosOperationFactory = new CosmosOperationFactory();

        public CosmosServiceOperationsProvider(
            List<ServiceOperation> triggerServiceOperations = null,
            InsensitiveDictionary<ServiceOperation> triggerServiceOperationManifest = null)
        {
            _serviceOperationsList = triggerServiceOperations ?? new List<ServiceOperation>();
            _apiOperationsList = triggerServiceOperationManifest ?? new InsensitiveDictionary<ServiceOperation>();
            
            _apiOperationsList.AddRange(new InsensitiveDictionary<ServiceOperation>
            {
                {"receiveDocument", ReceiveDocument.Operation},
                {"upsertDocument", UpsertAction.Operation},
                {"getDocument", GetAction.Operation},
                {"queryDocuments", QueryAction.Operation}
            });
            
            _serviceOperationsList.AddRange(new List<ServiceOperation>
            {
                ReceiveDocument.Operation.CloneWithManifest(ReceiveDocument.Manifest),
                UpsertAction.Operation.CloneWithManifest(UpsertAction.Manifest),
                QueryAction.Operation.CloneWithManifest(QueryAction.Manifest),
                GetAction.Operation.CloneWithManifest(GetAction.Manifest)
            });
        }

        public string GetBindingConnectionInformation(string operationId,
            InsensitiveDictionary<JToken> connectionParameters)
        {
            return ServiceOperationsProviderUtilities
                .GetRequiredParameterValue(
                    serviceId: ServiceId,
                    operationId: operationId,
                    parameterName: "connectionString",
                    parameters: connectionParameters)?
                .ToValue<string>();
        }
        
        public IEnumerable<ServiceOperation> GetOperations(bool expandManifest)
        {
            return expandManifest ? _serviceOperationsList : GetApiOperations();
        }

        public async Task<ServiceOperationResponse> InvokeOperation(string operationId,
            InsensitiveDictionary<JToken> connectionParameters,
            ServiceOperationRequest serviceOperationRequest)
        {
            var operation = _cosmosOperationFactory.GetCosmosOperation(operationId, 
                connectionParameters, serviceOperationRequest);
            
            var cosmosOperationResult = await operation.Invoke();

            return cosmosOperationResult;
        }
        
        private IEnumerable<ServiceOperation> GetApiOperations()
        {
            return _apiOperationsList.Values;
        }

        public ServiceOperationApi GetService()
        {
            return CosmosApiFactory.CosmosDbOperationApi;
        }
    }
}