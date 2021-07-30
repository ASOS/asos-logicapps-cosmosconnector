using System;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos
{
    public static class CosmosApiFactory
    {
        /// <summary>
        /// The Azure cosmos db API.
        /// </summary>
        public static readonly ServiceOperationApi CosmosDbOperationApi = new ServiceOperationApi
        {
            Name = "cosmos",
            Id = "/serviceProviders/cosmos",
            Type = DesignerApiType.ServiceProvider,
            Properties = new ServiceOperationApiProperties
            {
                BrandColor = 0xC4D5FF,
                Description = "Connect to Azure Cosmos db to work with data",
                DisplayName = "Cosmos Db",
                IconUri = new Uri(Constants.Icon),
                Capabilities = new[] { ApiCapability.Triggers, ApiCapability.Actions },
                ConnectionParameters = new ConnectionParameters
                {
                    ConnectionString = new ConnectionStringParameters
                    {
                        Type = ConnectionStringType.SecureString,
                        ParameterSource = ConnectionParameterSource.AppConfiguration,
                        UIDefinition = new UIDefinition
                        {
                            DisplayName = "Connection String",
                            Description = "Azure Cosmos db Connection String",
                            Tooltip = "Provide Azure Cosmos db Connection String",
                            Constraints = new Constraints
                            {
                                Required = "true",
                            },
                        },
                    },
                },
            },
        };
    }
}
