using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace PlayerSide
{
    public class RestService
    {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;

        public RestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<T>> RefreshDataAsync<T>(string uriResourcePath)
        {
            List<T> Items = new();

            Uri uri = new(string.Format($"{Constants.RestUrl}{uriResourcePath}"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<T>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@$"\tERROR {ex.Message} - {typeof(T)} - {uri}");
            }
            return Items;
        }

        public async Task<HttpResponseMessage> SaveDataAsync<T>(T item, string uriResourcePath, bool create=false)
        {
            Uri uri = new(string.Format($"{Constants.RestUrl}{uriResourcePath}"));
            HttpResponseMessage response = null;

            try
            {
                string json = JsonSerializer.Serialize<T>(item, _serializerOptions);
                StringContent content = new(json, Encoding.UTF8, "application/json");
                if (create)
                    response = await _client.PostAsync(uri, content);
                else 
                    response = await _client.PutAsync(uri, content);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@$"\tERROR {ex.Message} - {typeof(T)} - {uri} - {item}");
            }
            return response;
        }

        public async Task<HttpResponseMessage> DeleteDataAsync(string uriResourcePath)
        {
            Uri uri = new Uri(string.Format($"{Constants.RestUrl}{uriResourcePath}"));
            HttpResponseMessage response = null;

            try
            {
                response = await _client.DeleteAsync(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@$"\tERROR {ex.Message} - {uri}");
            }
            return response;
        }
    }
}
