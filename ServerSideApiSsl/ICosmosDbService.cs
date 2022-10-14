namespace ServerSideApiSsl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICosmosDbService<T>
    {
        Task<IEnumerable<T>> GetItemsAsync(string query);
        Task<T?> GetItemAsync(string id);
        Task AddItemAsync(T item, string id);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
