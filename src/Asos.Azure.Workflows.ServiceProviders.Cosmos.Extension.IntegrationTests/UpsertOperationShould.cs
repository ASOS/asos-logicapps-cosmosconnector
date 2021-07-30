using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Asos.Azure.Workflows.ServiceProviders.Cosmos;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Models;
using NUnit.Framework;
using Shouldly;
using static System.Text.Json.JsonSerializer;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests
{
    public class UpsertOperationShould
    {
        private readonly TestDataGenerator _testData;

        public UpsertOperationShould()
        {
            _testData = new TestDataGenerator();
        }
        
        [TestCase("")]
        [TestCase("{}")]
        public async Task Return_no_content_when_no_payload_specified(string payload)
        {
            var provider = new CosmosServiceOperationsProvider();
            var connectorParameters = new ConnectorParameters();
            
            connectorParameters.Parameters.Add("jsonDocument", payload);

            var result =
                await provider.InvokeOperation("upsertDocument",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.NoContent);
            result.Body.ToString().ShouldBeEquivalentTo("{}");
        }

        [Test]
        public async Task Insert_data_and_return_service_response()
        {
            var provider = new CosmosServiceOperationsProvider();
            var connectorParameters = new ConnectorParameters();

            var document = _testData.GetRandomTestData();
            connectorParameters.Parameters.Add("jsonDocument", Serialize(document));

            var result =
                await provider.InvokeOperation("upsertDocument",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.Created);

            var deserializedTestDocument = Deserialize<TestDocument>(result.Body.ToString());
            deserializedTestDocument.Field1.ShouldBeEquivalentTo(document.Field1);
        }

        [Test]
        public async Task Update_data_and_return_service_response()
        {
            var provider = new CosmosServiceOperationsProvider();
            var connectorParameters = new ConnectorParameters();

            var testDocument = await _testData.SetupData();
            var document = _testData.GetRandomTestData();
            document.Field1 = testDocument.Field1;
            document.Id = testDocument.Id;

            connectorParameters.Parameters.Add("jsonDocument", Serialize(document));

            var result =
                await provider.InvokeOperation("upsertDocument",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.OK);

            var deserializedTestDocument = Deserialize<TestDocument>(result.Body.ToString());
            deserializedTestDocument.Field1.ShouldBeEquivalentTo(document.Field1);
        }

        [TestCase("nonJsonString", "Invalid JSON")]
        [TestCase("<xml></xml>", "Invalid JSON")]
        [TestCase("{{}}", "Invalid JSON")]
        [TestCase("{ \"field1\": \"abc\"}", "One of the specified inputs is invalid")]
        public async Task Return_BadRequest_when_invalid_payload(string input, string errorMessage)
        {
            var provider = new CosmosServiceOperationsProvider();
            var connectorParameters = new ConnectorParameters();

            connectorParameters.Parameters.Add("jsonDocument", input);

            var result =
                await provider.InvokeOperation("upsertDocument",
                    connectorParameters.ConnectionParameters,
                    new ServiceOperationRequest(connectorParameters.Parameters));

            result.HttpStatus.ShouldBe(HttpStatusCode.BadRequest);
            var error = Deserialize<Error>(result.Body.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            error.Code.ShouldBeEquivalentTo(HttpStatusCode.BadRequest.ToString());
            error.Message.Contains(errorMessage);
        }
    }
}