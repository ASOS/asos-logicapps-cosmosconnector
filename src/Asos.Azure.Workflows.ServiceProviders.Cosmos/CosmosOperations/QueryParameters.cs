namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.CosmosOperations
{
    public class QueryParameters
    {
        public string SqlQuery { get; set; }
        public int? MaximumItemCount { get; set; }
        public string ContinuationToken { get; set; }
    }
}
