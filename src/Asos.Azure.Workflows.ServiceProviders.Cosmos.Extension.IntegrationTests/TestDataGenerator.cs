using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Asos.Azure.Workflows.ServiceProviders.Cosmos;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests.Models;
using System.Text.Json;
using System.Collections.Generic;

namespace Microsoft.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests
{
    public class TestDataGenerator
    {
        private const long TwentyFourHoursInSeconds = 86400;

        public async Task<TestDocument> SetupData()
        {
            var provider = new CosmosServiceOperationsProvider();
            var connectorParameters = new ConnectorParameters();
            
            TestDocument document = await CreateDocument(provider, connectorParameters);

            return document;
        }

        private async Task<TestDocument> CreateDocument(
            CosmosServiceOperationsProvider provider, 
            ConnectorParameters connectorParameters)
        {
            var document = GetRandomTestData();
            connectorParameters.Parameters.Add("jsonDocument", JsonSerializer.Serialize(document));

            await provider.InvokeOperation("upsertDocument",
                connectorParameters.ConnectionParameters,
                new ServiceOperationRequest(connectorParameters.Parameters));
            return document;
        }

        public async Task<IList<TestDocument>> SetupQueryableData(int count)
        {
            var provider = new CosmosServiceOperationsProvider();            

            var documents = new List<TestDocument>();

            ConnectorParameters connectorParameters;

            for (int i = 0; i < count; i++)
            {
                connectorParameters = new ConnectorParameters();

                var document = await CreateDocument(provider, connectorParameters);
                documents.Add(document);
            }          

            return documents;
        }

        public static string GetRandomString()
        {
            return new string(
                Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 13)
                    .Select(s =>
                    {
                        var cryptoResult = new byte[4];
                        using (var cryptoProvider = new RNGCryptoServiceProvider())
                            cryptoProvider.GetBytes(cryptoResult);

                        return s[new Random(BitConverter.ToInt32(cryptoResult, 0)).Next(s.Length)];
                    })
                    .ToArray());
        }

        public TestDocument GetRandomTestData()
        {
            return new TestDocument
            {
                Id = GetRandomString(),
                Field1 = GetRandomString(),
                Field2 = GetRandomString(),
                Timestamp = DateTime.UtcNow,
                Ttl = TwentyFourHoursInSeconds
            };
        }
    }
}
