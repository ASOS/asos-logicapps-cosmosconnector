using Asos.Azure.Workflows.ServiceProviders.Cosmos.Actions;
using NUnit.Framework;
using Shouldly;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.Tests
{
    public class UpsertActionShould
    {
        [Test]
        public void Return_service_manifest_details()
        {
            var manifest = UpsertAction.Manifest;

            manifest.Outputs.Properties.ShouldContainKey("body");
            manifest.Outputs.Properties.ShouldContainKey( "statusCode");

            manifest.Inputs.Properties.ShouldContainKey("databaseName");
            manifest.Inputs.Properties.ShouldContainKey("containerId");
            manifest.Inputs.Properties.ShouldContainKey("jsonDocument");

            manifest.Inputs.Required.ShouldContain("databaseName");
            manifest.Inputs.Required.ShouldContain("containerId");
            manifest.Inputs.Required.ShouldContain("jsonDocument");

            manifest.Connector.Name.ShouldBe("cosmos");
        }
    }
}