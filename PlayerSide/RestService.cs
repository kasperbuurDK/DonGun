﻿
using Android.Runtime;
using Android.Views.Accessibility;
using Java.Security;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PlayerSide
{
    public class RestService<T>
    {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public string UserName { get; set; } = "user";
        public string UserPassword { get; set; } = "password";
        public HttpResponseMessage Response { get; set; }
        public DateTime ModifiedOn { get; set; } = new DateTime();
        public string Logger { get; set; }
        public List<T> Items { get; private set; }

        public event EventHandler ResponseResived;
        public event EventHandler ResourceChanged;

        public RestService()
        {
            _client = new HttpClient();
            string authHeaer = Convert.ToBase64String(Encoding.ASCII.GetBytes(UserName + ":" + UserPassword));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaer);
            _client.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset(ModifiedOn);
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            StartRefTimer();
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
                AutoReset = true,
            };
            RefTimer.Elapsed += OnRefTimedEventWrapper;
            RefTimer.Enabled = true;
        }

        private async void OnRefTimedEventWrapper(Object source, System.Timers.ElapsedEventArgs e)
        {
            Uri uri = new(string.Format($"{Constants.RestUrl}{Constants.RestUriMod}{UserName}"));
            _client.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset(ModifiedOn);
            HttpResponseMessage _response = await _client.PostAsync(uri, null);
            if (_response.StatusCode != System.Net.HttpStatusCode.NotModified)
            {
                MainThread.BeginInvokeOnMainThread(() => ResourceChanged?.Invoke(this, EventArgs.Empty));
            }
        }
    }
}
