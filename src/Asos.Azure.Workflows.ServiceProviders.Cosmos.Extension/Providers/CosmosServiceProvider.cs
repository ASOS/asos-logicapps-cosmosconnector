using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Json;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Extension.Providers
{
    [Extension("CosmosServiceProvider", "CosmosServiceProvider")]
    public class CosmosServiceProvider : IExtensionConfigProvider
    {
        public CosmosServiceProvider(
            ServiceOperationsProvider serviceOperationsProvider,
            CosmosServiceOperationsProvider cosmosOperationsProvider)
        {
            serviceOperationsProvider.RegisterService(
                CosmosServiceOperationsProvider.ServiceId, 
                CosmosServiceOperationsProvider.ServiceName, 
                cosmosOperationsProvider);
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context.AddConverter<IReadOnlyList<Document>, JObject[]>(ConvertDocumentToJObject);
        }

        private static JObject[] ConvertDocumentToJObject(IReadOnlyList<Document> data)
        {
            return data.Select(doc => (JObject) doc.ToJToken()).ToArray();
        }
    }
}