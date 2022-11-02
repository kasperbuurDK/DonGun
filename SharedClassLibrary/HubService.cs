using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Input;

namespace SharedClassLibrary
{
    public class HubService<T>
    {
        // Fields
        private readonly HubConnection hubConnection;
        public string Logger { get; set; } = string.Empty;

        //Properties
        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
        public string AuthHeader { get; set; }
        public GameSessionOptions GameOptions { get; set; } = new();

        // Events
        public event EventHandler<HubEventArgs<T>>? PropertyChangedEvent;
        public event EventHandler<HubEventArgs<HubServiceException>>? ExceptionHandlerEvent;

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

            hubConnection.On<HubServiceException>("ExceptionHandler", msg =>
            {
                ExceptionHandlerEvent?.Invoke(this, new HubEventArgs<HubServiceException>() { Messege = msg });
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
        /// Join game room with sesstion key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task JoinRoom(string key)
        {
            GameOptions = new GameSessionOptions() { SessionKey = key };
            await hubConnection.SendAsync("JoinGameRoom", GameOptions);
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

    /// <summary>
    /// Exception contenxt resived from hub on errors.
    /// </summary>
    public class HubServiceException
    {
        public string Messege { get; set; } = string.Empty;
    }
}