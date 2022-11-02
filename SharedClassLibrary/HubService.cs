using Microsoft.AspNetCore.SignalR.Client;
using SharedClassLibrary.MessageStrings;
using System.Windows.Input;

namespace SharedClassLibrary
{
    public class HubService
    {
        // Fields
        private readonly HubConnection hubConnection;

        //Properties
        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
        public string? SessionKey { get; set; }
        public GameSessionOptions GameOptions { get; set; } = new();

        // Events
        public event EventHandler<HubEventArgs<FileUpdateMessage>>? FileEvent;
        public event EventHandler<HubEventArgs<StandardMessages>>? MoveEvent;
        public event EventHandler<HubEventArgs<StandardMessages>>? DiceEvent;
        public event EventHandler<HubEventArgs<StandardMessages>>? ErrorEvent;
        public event EventHandler<HubEventArgs<StandardMessages>>? UpdateEvent;
        public event EventHandler<HubEventArgs<HubServiceException>>? ExceptionHandlerEvent;

        // Constructor
        public HubService(string authHeader, string baseUrl, string hubUri, bool clientDon=false) 
        {
            hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{baseUrl}{hubUri}", options => options.Headers.Add("Authorization", $"Basic {authHeader}"))
                    .Build();

            if (clientDon)
            {
                hubConnection.On<StandardMessages>("MoveEvent", msg =>
                {
                    MoveEvent?.Invoke(this, new HubEventArgs<StandardMessages>() { Messege = msg });
                });
                hubConnection.On<StandardMessages>("DiceEvent", msg =>
                {
                    DiceEvent?.Invoke(this, new HubEventArgs<StandardMessages>() { Messege = msg });
                });
            }
            hubConnection.On<FileUpdateMessage>("FileEvent", msg =>
            {
                FileEvent?.Invoke(this, new HubEventArgs<FileUpdateMessage>() { Messege = msg });
            });
            hubConnection.On<StandardMessages>("ErrorEvent", msg =>
            {
                ErrorEvent?.Invoke(this, new HubEventArgs<StandardMessages>() { Messege = msg });
            });
            hubConnection.On<StandardMessages>("UpdateEvent", msg =>
            {
                UpdateEvent?.Invoke(this, new HubEventArgs<StandardMessages>() { Messege = msg });
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
            SessionKey = key;
            await hubConnection.SendAsync("JoinGameRoom", GameOptions);
        }

        /// <summary>
        /// Leave game room with given sesstion key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task LeaveRoom(string key)
        {
            GameOptions = new GameSessionOptions() { SessionKey = key };
            await hubConnection.SendAsync("LeaveGameRoom", GameOptions);
        }

        /// <summary>
        /// Send event to hub.
        /// </summary>
        /// <returns></returns>
        public async Task Send<T>(T msg) where T : Message
        {
            if (SessionKey is null)
                throw new ArgumentNullException(nameof(SessionKey));
            msg.SessionKey = SessionKey;
            string endpoint = msg.MsgType.ToString();
            await hubConnection.SendAsync(endpoint, msg);
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