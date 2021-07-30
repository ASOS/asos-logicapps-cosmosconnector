using System;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Swagger.Entities;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions
{
    public static class UpsertAction
    {
        public static readonly ServiceOperationManifest Manifest = new CosmosServiceOperationManifest
        {
            Outputs = new SwaggerSchema
            {
                Type = SwaggerSchemaType.Object,
                Properties = new OrdinalDictionary<SwaggerSchema>
                {
                    { 
                        "body", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.Object
                        }
                    },
                    {
                        "statusCode", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.Integer
                        }
                    }
                }                
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
                            Title = "Database name",
                            Description = "Database name",
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
                    {
                        "jsonDocument", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.String,
                            Title = "Json Document",
                            Description = "Json payload to insert into the collection",
                        }
                    }
                },
                Required = new[]
                {
                    "databaseName",
                    "containerId",
                    "jsonDocument"
                }
            },
            Connector = CosmosApiFactory.CosmosDbOperationApi
        };


        public static readonly ServiceOperation Operation = new ServiceOperation
        {
            Name = "upsertDocument",
            Id = "upsertDocument",
            Type = "upsertDocument",
            Properties = new ServiceOperationProperties
            {
                Api = CosmosApiFactory.CosmosDbOperationApi.GetFlattenedApi(),
                Summary = "Upsert Document",
                Description = "Creates a document if it doesn't exist, others updates the existing document",
                Visibility = Visibility.Important,
                OperationType = OperationType.ServiceProvider,
                BrandColor = 0x1C3A56,
                IconUri = new Uri(Constants.Icon),
                Capabilities = new[] { ApiCapability.Actions }
            },
        };
    }
}
