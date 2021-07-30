using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Asos.Azure.Workflows.ServiceProviders.Cosmos;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Helpers;
using Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Models;
using NUnit.Framework;
using Shouldly;
using static System.Text.Json.JsonSerializer;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests
{
    public class QueryOperationShould
    {
        private const int DefaultMaximumItemCount = 5;

        private readonly TestDataGenerator _testData;

        public QueryOperationShould()
        {
            _testData = new TestDataGenerator();
        }

        [Test]
        public async Task Query_data_and_return_service_response()
        {
            var provider = new CosmosServiceOperationsProvider();

            var testDocuments = await _testData.SetupQueryableData(DefaultMaximumItemCount);

            var queryParameters = new QueryParameters
            {
                SqlQuery = GenerateSqlQueryForGettingDocumentsByIds(testDocuments),
                MaximumItemCount = DefaultMaximumItemCount
            };

            var connectorParameters = new ConnectorParameters();
            connectorParameters.Parameters.AssignQueryParameters(queryParameters);

            var result =
                await provider.InvokeOperation("queryDocuments",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.OK);

            var data = result.Body;
            var deserializedResult = Deserialize<QueryResponse<TestDocument>>(data.ToString());

            deserializedResult.Documents.Select(i => i.Field1).ShouldBeEquivalentTo(testDocuments.Select(d => d.Field1));
        }

        [Test]
        public async Task Return_empty_result_when_no_matching_documents_exist()
        {
            var provider = new CosmosServiceOperationsProvider();

            var queryParameters = new QueryParameters
            {
                SqlQuery = "SELECT * FROM c WHERE c.id = 'i-dont-exist'",
                MaximumItemCount = DefaultMaximumItemCount
            };

            var connectorParameters = new ConnectorParameters();
            connectorParameters.Parameters.AssignQueryParameters(queryParameters);

            var result =
                await provider.InvokeOperation("queryDocuments",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.OK);

            var data = result.Body;

            var deserializedResult = Deserialize<QueryResponse<TestDocument>>(data.ToString());

            deserializedResult.Documents.ShouldBeEmpty();
            deserializedResult.ContinuationToken.ShouldBeNull();
        }

        [Test]
        public async Task Return_limited_to_max_item_list_and_continuationToken()
        {
            const int batchCount = 2;
            var provider = new CosmosServiceOperationsProvider();

            var testDocuments = await _testData.SetupQueryableData(DefaultMaximumItemCount * batchCount);

            var queryParameters = new QueryParameters
            {
                SqlQuery = GenerateSqlQueryForGettingDocumentsByIds(testDocuments),
                MaximumItemCount = DefaultMaximumItemCount
            };

            var connectorParameters = new ConnectorParameters();
            connectorParameters.Parameters.AssignQueryParameters(queryParameters);

            var result =
                await provider.InvokeOperation("queryDocuments",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.OK);

            var data = result.Body;
            var deserializedResult = Deserialize<QueryResponse<TestDocument>>(data.ToString());

            deserializedResult.ContinuationToken.ShouldNotBeNullOrEmpty();
            deserializedResult.Documents.Count.ShouldBeLessThanOrEqualTo(DefaultMaximumItemCount);
        }

        [Test]
        public async Task Return_batches_correctly_using_continuation_token()
        {
            const int batchCount = 2;
            const int timeoutMinutes = 2;
            var provider = new CosmosServiceOperationsProvider();

            var testDocuments = await _testData.SetupQueryableData(DefaultMaximumItemCount * batchCount);

            var queryParameters = new QueryParameters
            {
                SqlQuery = GenerateSqlQueryForGettingDocumentsByIds(testDocuments),
                MaximumItemCount = DefaultMaximumItemCount
            };

            ConnectorParameters connectorParameters;

            var stopTime = DateTime.UtcNow.AddMinutes(timeoutMinutes);

            do
            {
                connectorParameters = new ConnectorParameters();
                connectorParameters.Parameters.AssignQueryParameters(queryParameters);

                var result =
                    await provider.InvokeOperation("queryDocuments",
                        connectorParameters.ConnectionParameters,
                        new ServiceOperationRequest(connectorParameters.Parameters));

                result.HttpStatus.ShouldBe(HttpStatusCode.OK);

                var data = result.Body;
                var deserializedResult = Deserialize<QueryResponse<TestDocument>>(data.ToString());

                deserializedResult.Documents.Select(d => d.Field1).ShouldBeSubsetOf(testDocuments.Select(d => d.Field1));

                queryParameters.ContinuationToken = deserializedResult.ContinuationToken;

            } while (queryParameters.ContinuationToken != null && stopTime > DateTime.UtcNow);

            queryParameters.ContinuationToken.ShouldBeNull();
        }

        [Test]
        public async Task Return_BadRequest_when_invalid_query()
        {
            var provider = new CosmosServiceOperationsProvider();

            var queryParameters = new QueryParameters
            {
                SqlQuery = "Invalid query",
                MaximumItemCount = DefaultMaximumItemCount
            };

            var connectorParameters = new ConnectorParameters();
            connectorParameters.Parameters.AssignQueryParameters(queryParameters);

            var result =
                await provider.InvokeOperation("queryDocuments",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.BadRequest);
            var error = Deserialize<Error>(result.Body.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            error.Code.ShouldBeEquivalentTo(HttpStatusCode.BadRequest.ToString());
        }

        private static string GenerateSqlQueryForGettingDocumentsByIds(IList<TestDocument> testDocuments)
        {
            return $"SELECT * FROM c WHERE c.id IN (" +
               $"{string.Join(",", testDocuments.Select(d => $"'{d.Id}'").ToList())})";
        }
    }
}