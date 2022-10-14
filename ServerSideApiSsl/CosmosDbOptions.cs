namespace ServerSideApiSsl
{
    public class CosmosDbOptions
    {
        public const string CosmosDb = "CosmosDb";

        public string EndpointUri { get; set; } = string.Empty;
        public string PrimaryKey { get; set; } = string.Empty;
    }
}
