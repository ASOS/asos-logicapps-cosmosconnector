using Microsoft.Azure.Workflows.ServiceProviders.WebJobs.Abstractions.Providers;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Extension.Providers
{
    public class CosmosServiceOperationsTriggerProvider :
        CosmosServiceOperationsProvider, IServiceOperationsTriggerProvider
    {
        public CosmosServiceOperationsTriggerProvider()
            : base()
        {
        }

        public string GetFunctionTriggerType()
        {
            return "CosmosTrigger";
        }
    }
}