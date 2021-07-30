using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using static Microsoft.Azure.Workflows.ServiceProviders.Abstractions.InputsLocation;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions
{
    public class CosmosServiceOperationManifest : ServiceOperationManifest
    {
        public CosmosServiceOperationManifest()
        {
            ConnectionReference = new ConnectionReferenceFormat
            {
                ReferenceKeyFormat = ConnectionReferenceKeyFormat.ServiceProvider,
            };

            Settings = new OperationManifestSettings
            {
                SecureData = new OperationManifestSettingWithOptions<SecureDataOptions>(),
                TrackedProperties = new OperationManifestSetting
                {
                    Scopes = new[] { OperationScope.Action },
                },
            };

            InputsLocation = new[]
            {
                Microsoft.Azure.Workflows.ServiceProviders.Abstractions.InputsLocation.Inputs,
                Parameters,
            };
        }
    
    }
}