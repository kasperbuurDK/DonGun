namespace ServerSideApiSsl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISqlDbService<T> 
    {
        bool Authenticate(string username, string password);
        Task<T?> GetItemAsync(string id);
        Task AddItemAsync(T item, string id);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
