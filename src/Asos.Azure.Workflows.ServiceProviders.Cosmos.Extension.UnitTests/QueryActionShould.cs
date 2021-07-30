using Asos.Azure.Workflows.ServiceProviders.Cosmos;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions;
using NUnit.Framework;
using Shouldly;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.Tests
{
    public class QueryActionShould
    {
        [Test]
        public void Return_service_manifest_details()
        {
            var manifest = QueryAction.Manifest;

            manifest.Outputs.Properties.ShouldContainKey("body");
            manifest.Outputs.Properties.ShouldContainKey("statusCode");

            manifest.Outputs.Properties["body"].Properties.ShouldContainKey("value");
            manifest.Outputs.Properties["body"].Properties.ShouldContainKey("continuationToken");

            manifest.Inputs.Properties.ShouldContainKey("databaseName");
            manifest.Inputs.Properties.ShouldContainKey("containerId");
            manifest.Inputs.Properties.ShouldContainKey("sqlQuery");
            manifest.Inputs.Properties.ShouldContainKey("maximumItemCount");
            manifest.Inputs.Properties.ShouldContainKey("continuationToken");


            manifest.Inputs.Required.ShouldContain("databaseName");
            manifest.Inputs.Required.ShouldContain("containerId");
            manifest.Inputs.Required.ShouldContain("sqlQuery");
            manifest.Inputs.Required.ShouldContain("maximumItemCount");

            manifest.Connector.Name.ShouldBe("cosmos");
        }

        [Test]
        public void Return_operation_details()
        {
            var operationDetails = QueryAction.Operation;

            operationDetails.Id.ShouldBe("queryDocuments");
            operationDetails.Name.ShouldBe("queryDocuments");
            operationDetails.Type.ShouldBe("queryDocuments");

            operationDetails.Properties.Api.ShouldBeEquivalentTo(
                CosmosApiFactory.CosmosDbOperationApi.GetFlattenedApi());
        }
    }
}