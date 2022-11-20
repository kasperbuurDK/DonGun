using Microsoft.AspNetCore.SignalR.Client;
using SharedClassLibrary.MessageStrings;
using System.Net;
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
        public event EventHandler<HubEventArgs<MoveMessage>>? MoveEvent;
        public event EventHandler<HubEventArgs<DiceRolledMessage>>? DiceEvent;
        public event EventHandler<HubEventArgs<StandardMessages>>? ErrorEvent;
        public event EventHandler<HubEventArgs<UpdateMessage>>? UpdateEvent;
        public event EventHandler<HubEventArgs<HubServiceException>>? ExceptionHandlerEvent;
        public event EventHandler<HubEventArgs<StartGameMessage>>? StartGameEvetnHandler;


        // Constructor
        public HubService(string authHeader, string baseUrl, string hubUri, bool clientDon = false)
        {
            hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{baseUrl}{hubUri}",
                    options => {
                    options.Headers.Add("Authorization", $"Basic {authHeader}");
                    options.Headers.Add("Access-Control-Allow-Origin", "*");
                    })
                    .Build();

            if (clientDon)
            {
                hubConnection.On<MoveMessage>("MoveEvent", msg =>
                {
                    MoveEvent?.Invoke(this, new HubEventArgs<MoveMessage>() { Messege = msg });
                });
                hubConnection.On<DiceRolledMessage>("DiceEvent", msg =>
                {
                    DiceEvent?.Invoke(this, new HubEventArgs<DiceRolledMessage>() { Messege = msg });
                });
            }
            hubConnection.On<FileUpdateMessage>("FileEvent", msg =>
            {
                FileEvent?.Invoke(this, new HubEventArgs<FileUpdateMessage>() { Messege = msg });
            });

            // Error from Don
            hubConnection.On<StandardMessages>("ErrorEvent", msg =>
            {
                ErrorEvent?.Invoke(this, new HubEventArgs<StandardMessages>() { Messege = msg });
            });
            hubConnection.On<UpdateMessage>("UpdateEvent", msg =>
            {
                UpdateEvent?.Invoke(this, new HubEventArgs<UpdateMessage>() { Messege = msg });
            });

            // Exception from hub system
            hubConnection.On<HubServiceException>("ExceptionHandler", msg =>
            {
                ExceptionHandlerEvent?.Invoke(this, new HubEventArgs<HubServiceException>() { Messege = msg });
            });

            hubConnection.On<StartGameMessage>("StartGame", msg =>
            {
                StartGameEvetnHandler?.Invoke(this, new HubEventArgs<StartGameMessage>() { Messege = msg });
            });

        }

        // Methods
        /// <summary>
        /// Connect to Hub
        /// </summary>
        /// <returns></returns>
        public async Task Initialise()
        {
            if (!IsConnected)
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
        /// Join game room with sesstion key and give selectet sheet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task JoinRoom(string key, Player p)
        {
            GameOptions = new GameSessionOptions() { SessionKey = key , Sheet = p};
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
        public int Code { get; set; } = (int)HttpStatusCode.OK;
        public string ActionName { get; set; } = string.Empty;
    }
}