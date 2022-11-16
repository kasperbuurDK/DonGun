
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace SharedClassLibrary
{
    public class RestService<TStruct, TValue> where TStruct : class, new()
    {
        // Fields
        private readonly HttpClient _client;

        // HTTP related properties
        public HttpResponseMessage? Response { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string AuthHeader { get; private set; } = string.Empty;
        public DateTime ModifiedOn { get; set; } = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        public TStruct? ReturnStruct { get; private set; }
        public Uri BaseUrl { get; set; }

        // Class related properties
        public string Logger { get; set; } = string.Empty;
        public Exception? CatchedException { get; private set; }

        // Event
        public event EventHandler? ResponseResived;

        // Constructors
        public RestService(Uri baseUrl, string user, string password) : this(baseUrl, Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + password)))
        {
            UserName = user;           
        }

        public RestService(Uri baseUrl, string authHeader)
        {
            _client = new HttpClient();
            AuthHeader = authHeader;
            BaseUrl = baseUrl;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthHeader);
            _client.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset(ModifiedOn, new TimeSpan(0));
        }

        /// <summary>
        /// Quarry server to get new data object.
        /// </summary>
        /// <param name="uriPath"></param>
        public async Task RefreshDataAsync(string uriPath)
        {
            ReturnStruct = new();
            Uri uri = new(string.Format($"{BaseUrl}{uriPath}"));

            try
            {
                Response = await _client.GetAsync(uri);
                if (Response.IsSuccessStatusCode)
                {
                    string content = await Response.Content.ReadAsStringAsync();
                    ReturnStruct = content.JsonToType<TStruct>();
                    //ReturnStruct = JsonSerializer.Deserialize<TStruct>(content, _serializerOptions);
                    HttpHeaders headers = Response.Headers;
                    if (headers.TryGetValues("Last-Modified", out var values))
                    {
                        try
                        {
                            ModifiedOn = DateTime.ParseExact(values.First(),
                                "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                CultureInfo.InvariantCulture.DateTimeFormat,
                                DateTimeStyles.AssumeUniversal);
                        }
                        catch (FormatException ex)
                        {
                            ModifiedOn = new DateTime();
                            Logger = string.Format($"ERROR {ex.Message} - {uri}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger = string.Format($"ERROR {ex.Message} - {typeof(TValue)} - {uri}");
                CatchedException = ex;
            }
        }

        /// <summary>
        /// Quarry server to post/save item object. If crate is true item is put onto the server.
        /// </summary>
        /// <param name="item">Item of type /<T/></param>
        /// <param name="uriResourcePath"></param>
        /// <param name="create">True = Perform put request</param>
        /// <returns></returns>
        public async Task SaveDataAsync(TValue item, string uriResourcePath, bool create = false)
        {
            Uri uri = new(string.Format($"{BaseUrl}{uriResourcePath}"));
            Response = null;

            try
            {
                string json = item.TypeToJson();
                //string json = JsonSerializer.Serialize(item, _serializerOptions);
                StringContent content = new(json, Encoding.UTF8, "application/json");
                if (!create)
                    Response = await _client.PostAsync(uri, content);
                else
                    Response = await _client.PutAsync(uri, content);
                RaiseEvent(ResponseResived);
            }
            catch (Exception ex)
            {
                Logger = string.Format($"ERROR {ex.Message} - {typeof(TValue)} - {uri} - {item}");
                CatchedException = ex;
            }
        }

        /// <summary>
        /// Quarry server to delete item object.
        /// </summary>
        /// <param name="uriResourcePath"></param>
        public async Task DeleteDataAsync(TValue item, string uriResourcePath)
        {
            Uri uri = new(string.Format($"{BaseUrl}{uriResourcePath}"));
            Response = null;

            try
            {
                string json = item.TypeToJson();
                StringContent content = new(json, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new(HttpMethod.Delete, uri)
                {
                    Content = content
                };
                Response = await _client.SendAsync(request);
                RaiseEvent(ResponseResived);
            }
            catch (Exception ex)
            {
                Logger = string.Format($"ERROR {ex.Message} - {uri}");
                CatchedException = ex;
            }
        }

        private void RaiseEvent(EventHandler? handler)
        {
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
