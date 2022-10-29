using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Input;

namespace SharedClassLibrary
{
    public class HubService<T>
    {
        // Fields
        private readonly HubConnection hubConnection;

        //Properties
        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
        public string AuthHeader { get; set; }

        // Events
        public event EventHandler<HubEventArgs<T>>? PropertyChangedEvent;

        // Constructor
        public HubService(string authHeader, string baseUrl, string hubUri) 
        {
            AuthHeader = authHeader;
            hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{baseUrl}{hubUri}", options => options.Headers.Add("Authorization", $"Basic {AuthHeader}"))
                    .Build();

            hubConnection.On<T>("ReceiveUpdateEvent", msg =>
            {
                PropertyChangedEvent?.Invoke(this, new HubEventArgs<T>() { Messege = msg });
            });
        }

        // Methods
        /// <summary>
        /// Connect to Hub
        /// </summary>
        /// <returns></returns>
        public async Task Initialise()
        {
            await hubConnection.StartAsync();
        }

        /// <summary>
        /// Send event to hub.
        /// </summary>
        /// <returns></returns>
        public async Task Send(T msg)
        {
            await hubConnection.SendAsync("SendUpdateEvent", msg);
        }
    }

    /// <summary>
    /// Parse Hub message via event arguments
    /// </summary>
    public class HubEventArgs<T> : EventArgs
    {
        public T? Messege { get; set; }
    }
}