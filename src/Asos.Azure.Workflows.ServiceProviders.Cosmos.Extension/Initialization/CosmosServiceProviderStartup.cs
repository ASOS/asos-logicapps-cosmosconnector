using Asos.Azure.Workflows.ServiceProviders.Cosmos.Extension.Initialization;
using Asos.Azure.Workflows.ServiceProviders.Cosmos.Extension.Providers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: WebJobsStartup(typeof(CosmosServiceProviderStartup))]
namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Extension.Initialization
{
    public class CosmosServiceProviderStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<CosmosServiceProvider>();
            builder.Services.TryAddSingleton<CosmosServiceOperationsProvider>();
        }
    }
}
