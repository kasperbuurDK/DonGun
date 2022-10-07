﻿
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PlayerSide
{
    public class RestService<T>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;

        public HttpResponseMessage Response { get; set; }
        public string UserName { get; set; }
        public DateTime ModifiedOn { get; set; } = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        public string AuthHeader { get; private set; }
        public List<T> Items { get; private set; }

        public Action CallBackRefreshFunc { get; set; }
        private BackgroundWorker Worker { get; set; }

        public string Logger { get; set; }

        public RestService(string user, string password) : this(Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + password)))
        {
            UserName = user;           
        }
        public RestService(string authHeader)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            _client.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset(ModifiedOn, new TimeSpan(0));
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            AuthHeader = authHeader;
            StartRefTimer();
        }

        public async Task RefreshDataAsync(string uriResourcePath)
        {
            Items = new();

            Uri uri = new(string.Format($"{Constants.RestUrl}{uriResourcePath}"));
            try
            {
                // Localhosting return exception...
                Response = await _client.GetAsync(uri);
                if (Response.IsSuccessStatusCode)
                {
                    string content = await Response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<T>>(content, _serializerOptions);
                    HttpHeaders headers = Response.Headers;
                    if (headers.TryGetValues("Last-Modified", out IEnumerable<string> values))
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
                // TODO: Handel exseptions better!
                Logger = string.Format($"ERROR {ex.Message} - {typeof(T)} - {uri}");
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(MainThreadCode);
            }
        }

        public async Task SaveDataAsync(T item, string uriResourcePath, bool create = false)
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
            finally
            {
                MainThread.BeginInvokeOnMainThread(MainThreadCode);
            }
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
            finally
            {
                MainThread.BeginInvokeOnMainThread(MainThreadCode);
            }
        }

        private void MainThreadCode()
        {
            ResponseResived?.Invoke(this, EventArgs.Empty);
        }

        private void StartRefTimer()
        {
            System.Timers.Timer RefTimer = new()
            {
                Interval = 2000,
                AutoReset = false,
            };
            RefTimer.Elapsed += OnRefTimedEventWrapper;
            RefTimer.Enabled = true;
            //RefTimer.Start();
        }

        private async void OnRefTimedEventWrapper(object sender, System.Timers.ElapsedEventArgs e)
        {
            Uri uri = new(string.Format($"{Constants.RestUrl}{Constants.RestUriMod}{UserName}"));
            _client.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset(ModifiedOn);
            HttpResponseMessage _response = await _client.PostAsync(uri, null);
            if (_response.StatusCode != System.Net.HttpStatusCode.NotModified)
            {
                if (CallBackRefreshFunc is not null && await CallBackRefreshFunc())
                    MainThread.BeginInvokeOnMainThread(() => ResourceChanged?.Invoke(this, EventArgs.Empty));    
            }
            //((System.Timers.Timer)sender).Start();
        }
    }
}
