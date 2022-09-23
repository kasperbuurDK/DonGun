using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PlayerSide
{
    public class RestService<T>
    {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public string UserName { get; set; } = "user";
        public string UserPassword { get; set; } = "password1";
        public HttpResponseMessage Response { get; set; }
        public string Logger { get; set; }
        public List<T> Items { get; private set; }

        public event EventHandler ResponseResived;

        public RestService()
        {
            _client = new HttpClient();
            string authHeaer = Convert.ToBase64String(Encoding.ASCII.GetBytes(UserName + ":" + UserPassword));  
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaer);
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task RefreshDataAsync(string uriResourcePath)
        {
            Items = new();

            Uri uri = new(string.Format($"{Constants.RestUrl}{uriResourcePath}"));
            try
            {
                Response = await _client.GetAsync(uri);
                if (Response.IsSuccessStatusCode)
                {
                    string content = await Response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<T>>(content, _serializerOptions);
                }
                if (Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    Items = null;
            }
            catch (Exception ex)
            {
                Logger = string.Format($"ERROR {ex.Message} - {typeof(T)} - {uri}");
            }
            RaiseEvent(ResponseResived);
        }

        public async Task SaveDataAsync(T item, string uriResourcePath, bool create=false)
        {
            Uri uri = new(string.Format($"{Constants.RestUrl}{uriResourcePath}"));
            Response = null;

            try
            {
                string json = JsonSerializer.Serialize<T>(item, _serializerOptions);
                StringContent content = new(json, Encoding.UTF8, "application/json");
                if (create)
                    Response = await _client.PostAsync(uri, content);
                else
                    Response = await _client.PutAsync(uri, content);                
            }
            catch (Exception ex)
            {
                Logger = string.Format($"ERROR {ex.Message} - {typeof(T)} - {uri} - {item}");
            }
            RaiseEvent(ResponseResived);
        }

        public async Task DeleteDataAsync(string uriResourcePath)
        {
            Uri uri = new(string.Format($"{Constants.RestUrl}{uriResourcePath}"));
            Response = null;

            try
            {
                Response = await _client.DeleteAsync(uri);
            }
            catch (Exception ex)
            {
                Logger = string.Format($"ERROR {ex.Message} - {uri}");
            }
            RaiseEvent(ResponseResived);
        }

        private void RaiseEvent(EventHandler handler)
        {
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
