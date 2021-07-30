using System;
using System.Data.Common;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Infrastructure
{
    internal class CosmosDbConnectionStringDetails
    {
        public CosmosDbConnectionStringDetails(string connectionString)
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            
            if (builder.TryGetValue("AccountEndpoint", out var uri))
            {
                ServiceEndpoint = uri.ToString();
            }

            if (ServiceEndpoint == null)
            {
                throw new NullReferenceException("AccountEndpoint was not extracted from cosmos connection string, ensure details are correct");
            }
        }

        public string ServiceEndpoint { get; set; }
    }
}