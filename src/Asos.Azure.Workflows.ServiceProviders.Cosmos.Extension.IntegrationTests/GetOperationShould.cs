using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Asos.Azure.Workflows.ServiceProviders.Cosmos;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Models;
using NUnit.Framework;
using Shouldly;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests
{
    public class GetOperationShould
    {
        private readonly TestDataGenerator _testData;

        public GetOperationShould()
        {
            _testData = new TestDataGenerator();
        }
        
        [Test]
        public async Task Get_data_and_return_service_response()
        {
            var provider = new CosmosServiceOperationsProvider();

            var testDocument = await _testData.SetupData();

            var connectorParameters = new ConnectorParameters();
            connectorParameters.Parameters.Add("documentId", testDocument.Id);
            connectorParameters.Parameters.Add("partitionKey", testDocument.Field1);

            var result =
                await provider.InvokeOperation("getDocument",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.OK);
            
            var data = result.Body;
            var deserializedResult = JsonSerializer.Deserialize<TestDocument>(data.ToString());

            deserializedResult.Field1.ShouldBeEquivalentTo(testDocument.Field1);
        }
        
        [Test]
        public async Task Return_empty_data_and_not_found_status_code_for_missing_data()
        {
            var provider = new CosmosServiceOperationsProvider();

            var connectorParameters = new ConnectorParameters();
            connectorParameters.Parameters.Add("documentId", "i-dont-exist");
            connectorParameters.Parameters.Add("partitionKey", "neither-do-i");

            var result = await provider.InvokeOperation("getDocument",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.OK);
            var error = JsonSerializer.Deserialize<Error>(result.Body.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            error.Code.ShouldBeEquivalentTo(HttpStatusCode.NotFound.ToString());
            error.Message.ShouldContain("Response status code does not indicate success: NotFound (404)");
        }
    }
}