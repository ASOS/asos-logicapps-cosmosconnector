namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Configuration
{
    public class DocumentDbConfiguration
    {
        public DocumentDbConfiguration(string connectionString, string databaseName, string containerId)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            ContainerId = containerId;
        }

        public string ConnectionString { get; }
        public string DatabaseName { get; }
        public string ContainerId { get; }
    }
}