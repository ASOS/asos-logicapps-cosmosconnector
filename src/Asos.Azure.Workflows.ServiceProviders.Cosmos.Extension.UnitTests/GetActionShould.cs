using Asos.Azure.Workflows.ServiceProviders.Cosmos;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions;
using NUnit.Framework;
using Shouldly;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.Tests
{
    public class GetActionShould
    {
        [Test]
        public void Return_service_manifest_details()
        {
            var manifest = GetAction.Manifest;
            
            manifest.Outputs.Properties.ShouldContainKey("body");
            manifest.Outputs.Properties.ShouldContainKey("statusCode");

            manifest.Inputs.Properties.ShouldContainKey("databaseName");
            manifest.Inputs.Properties.ShouldContainKey("containerId");
            manifest.Inputs.Properties.ShouldContainKey("partitionKey");
            manifest.Inputs.Properties.ShouldContainKey("documentId");

            manifest.Inputs.Required.ShouldContain("databaseName");
            manifest.Inputs.Required.ShouldContain("containerId");
            manifest.Inputs.Required.ShouldContain("partitionKey");
            manifest.Inputs.Required.ShouldContain("documentId");

            manifest.Connector.Name.ShouldBe("cosmos");
        }

        [Test]
        public void Return_operation_details()
        {
            var operationDetails = GetAction.Operation;
            
            operationDetails.Id.ShouldBe("getDocument");
            operationDetails.Name.ShouldBe("getDocument");
            operationDetails.Type.ShouldBe("getDocument");
            
            operationDetails.Properties.Api.ShouldBeEquivalentTo(
                CosmosApiFactory.CosmosDbOperationApi.GetFlattenedApi());
        }
    }
}