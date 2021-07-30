using System;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Swagger.Entities;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions
{
    public static class GetAction
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
                        "partitionKey", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.String,
                            Title = "Partition Key",
                            Description = "Partition Key"
                        }
                    },                    
                    {
                        "documentId", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.String,
                            Title = "Document Id",
                            Description = "Document Id",
                        }
                    }
                },
                Required = new[]
                {
                    "databaseName",
                    "containerId",
                    "partitionKey",
                    "documentId"
                }
            },
            Connector = CosmosApiFactory.CosmosDbOperationApi
        };

        public static readonly ServiceOperation Operation = new ServiceOperation
        {
            Name = "getDocument",
            Id = "getDocument",
            Type = "getDocument",
            Properties = new ServiceOperationProperties
            {
                Api = CosmosApiFactory.CosmosDbOperationApi.GetFlattenedApi(),
                Summary = "Get document",
                Description = "Get a single document by partition and document id",
                Visibility = Visibility.Important,
                OperationType = OperationType.ServiceProvider,
                BrandColor = 0x1C3A56,
                IconUri = new Uri(Constants.Icon),
                Capabilities = new[] { ApiCapability.Actions }
            },
        };
    }
}
