using System;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Swagger.Entities;
using Newtonsoft.Json.Linq;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Triggers
{
    public static class ReceiveDocument
    {
        public static readonly ServiceOperationManifest Manifest = new ServiceOperationManifest
        {
            ConnectionReference = new ConnectionReferenceFormat
            {
                ReferenceKeyFormat = ConnectionReferenceKeyFormat.ServiceProvider,
            },
            Settings = new OperationManifestSettings
            {
                SecureData = new OperationManifestSettingWithOptions<SecureDataOptions>(),
                TrackedProperties = new OperationManifestSetting
                {
                    Scopes = new[] { OperationScope.Trigger },
                },
            },
            InputsLocation = new[]
            {
                InputsLocation.Inputs,
                InputsLocation.Parameters,
            },
            Outputs = new SwaggerSchema
            {
                Type = SwaggerSchemaType.Object,
                Properties = new OrdinalDictionary<SwaggerSchema>
                {
                    {
                        "body", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.Array,
                            Title = "Receive document",
                            Description = "Receive document description",
                            Items = new SwaggerSchema
                            {
                                Type = SwaggerSchemaType.Object,
                                Properties = new OrdinalDictionary<SwaggerSchema>
                                {
                                    {
                                        "contentData", new SwaggerSchema
                                        {
                                            Type = SwaggerSchemaType.String,
                                            Title = "Content",
                                            Format = "byte",
                                            Description = "content",
                                        }
                                    },
                                    {
                                        "Properties", new SwaggerSchema
                                        {
                                            Type = SwaggerSchemaType.Object,
                                            Title = "documentProperties",
                                            AdditionalProperties = new JObject
                                            {
                                                { "type", "object" },
                                                { "properties", new JObject { } },
                                                { "required", new JObject { } },
                                            },
                                            Description = "document data properties",
                                        }
                                    },
                                },
                            },
                        }
                    },
                },
            },
            Inputs = new SwaggerSchema
            {
                Type = SwaggerSchemaType.Object,
                Properties = new OrdinalDictionary<SwaggerSchema>
                {
                    {
                        "databaseName", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.String,
                            Title = "database name",
                            Description = "database name",
                        }
                    },
                    {
                         "containerId", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.String,
                            Title = "Container Id",
                            Description = "Container Id",
                        }
                    },
                },
                Required = new[]
                {
                    "databaseName",
                    "containerId"
                },
            },
            Connector = CosmosApiFactory.CosmosDbOperationApi,
            Trigger = TriggerType.Batch,
            Recurrence = new RecurrenceSetting
            {
                Type = RecurrenceType.None,
            },
        };
        
        public static readonly ServiceOperation Operation = new ServiceOperation
        {
            Name = "receiveDocument",
            Id = "receiveDocument",
            Type = "receiveDocument",
            Properties = new ServiceOperationProperties
            {
                Api = CosmosApiFactory.CosmosDbOperationApi.GetFlattenedApi(),
                Summary = "receive document",
                Description = "receive document",
                Visibility = Visibility.Important,
                OperationType = OperationType.ServiceProvider,
                BrandColor = 0x1C3A56,
                IconUri = new Uri(Constants.Icon),
                Trigger = TriggerType.Batch,
                Capabilities = new[] { ApiCapability.Triggers }
            },
        };

    }
}
