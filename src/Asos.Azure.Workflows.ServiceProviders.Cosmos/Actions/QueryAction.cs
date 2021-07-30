using System;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Swagger.Entities;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions
{
    public static class QueryAction
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
                            Type = SwaggerSchemaType.Object,
                            Properties = new OrdinalDictionary<SwaggerSchema>
                            {
                                {
                                    "value", new SwaggerSchema
                                    {
                                        Type = SwaggerSchemaType.Array,
                                        Items = new SwaggerSchema
                                        {
                                           Type = SwaggerSchemaType.String
                                        }
                                    }
                                },
                                {
                                    "continuationToken", new SwaggerSchema
                                    {
                                        Type = SwaggerSchemaType.String
                                    }
                                },
                            },
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
                        "sqlQuery", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.String,
                            Title = "SQL Query",
                            Description = "SQL Query",
                        }
                    },
                    {
                        "maximumItemCount", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.Integer,
                            Title = "Max Item Count",
                            Description = "An integer indicating the maximum number of items to be returned per page. Default is 1000",
                        }
                    },
                    {
                        "continuationToken", new SwaggerSchema
                        {
                            Type = SwaggerSchemaType.String,
                            Title = "Continuation Token",
                            Description = "A token to fetch additional results",
                        }
                    }
                },
                Required = new[]
                {
                    "databaseName",
                    "containerId",
                    "sqlQuery",
                    "maximumItemCount"
                }
            },
            Connector = CosmosApiFactory.CosmosDbOperationApi
        };

        public static readonly ServiceOperation Operation = new ServiceOperation
        {
            Name = "queryDocuments",
            Id = "queryDocuments",
            Type = "queryDocuments",
            Properties = new ServiceOperationProperties
            {
                Api = CosmosApiFactory.CosmosDbOperationApi.GetFlattenedApi(),
                Summary = "Query data",
                Description = "Perform query against Cosmos db",
                Visibility = Visibility.Important,
                OperationType = OperationType.ServiceProvider,
                BrandColor = 0x1C3A56,
                IconUri = new Uri(Constants.Icon),
                Capabilities = new[] { ApiCapability.Actions }
            }
        };
    }
}
