using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace ServerSideApiSsl
{
    public class CosmosDbService<T> : ICosmosDbService<T>
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public static CosmosDbService<T> InitializeCosmosClientInstance(IConfiguration configuration)
        {
            string endpointUri = configuration["EndpointUri"];
            string primaryKey = configuration["PrimaryKey"];
            string databaseName = configuration["DatabaseName"];
            string containerName = configuration["ContainerName"]; // Missing

            CosmosClient client = new(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = "ServerSideApiSsl" });
            CosmosDbService<T> cosmosDbService = new (client, databaseName, containerName);

            return cosmosDbService;
        }

        private CosmosDbService( CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(T item, string Id)
        {
            await _container.CreateItemAsync(item, new PartitionKey(Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<T?> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string queryString)
        {
            FeedIterator<T> query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }
    }
}
