using System.Linq;
using Asos.Azure.Workflows.ServiceProviders.Cosmos;
using NUnit.Framework;
using Shouldly;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.Tests
{
    public class CosmosDbTriggerServiceOperationProviderShould
    {
        [Test]
        public void Contain_Expected_Number_Of_Operations()
        {
            var provider = new CosmosServiceOperationsProvider();

            var operations = provider.GetOperations(false);
            var operationsList = operations.ToList();

            operationsList.Count.ShouldBe(4);
        }
    }
}