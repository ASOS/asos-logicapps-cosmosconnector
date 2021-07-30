using System;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.Tests
{
    public class CosmosOperationFactoryShould
    {
        [TestCase("getDocument", typeof(GetOperation))]
        [TestCase("queryDocuments", typeof(QueryOperation))]
        [TestCase("upsertDocument", typeof(UpsertOperation))]
        public void Return_The_Expected_Type(string operationId, Type expectedType)
        {
            var operationFactory = new CosmosOperationFactory();
            var parameters = new InsensitiveDictionary<JToken>();
            var result =
                operationFactory.GetCosmosOperation(operationId, parameters, new ServiceOperationRequest(parameters));

            Assert.AreEqual(expectedType.FullName, result.GetType().FullName);
        }
        
        [Test]
        public void Throws_Exception_For_Unknown_Type()
        {
            var operationFactory = new CosmosOperationFactory();
            var parameters = new InsensitiveDictionary<JToken>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                operationFactory.GetCosmosOperation("invalidOperation", parameters,
                    new ServiceOperationRequest(parameters));
            });
        }
    }
}