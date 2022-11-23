using Microsoft.AspNetCore.SignalR.Client;
using SharedClassLibrary.MessageStrings;
using System.Net;

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
        public event EventHandler<HubEventArgs<ActionMessage>>? ActionEvent;
        public event EventHandler<HubEventArgs<UpdateMessage>>? UpdateEvent;
        public event EventHandler<HubEventArgs<HubServiceException>>? ExceptionHandlerEvent;
        public event EventHandler<HubEventArgs<StartGameMessage>>? StartGame;
        public event EventHandler<HubEventArgs<GameSessionOptions>>? JoinEvent;
        public event EventHandler<HubEventArgs<GameSessionOptions>>? LeaveEvent;
        public event EventHandler<HubEventArgs<NewTurnMessage>>? NewTurnEvent;
        public event EventHandler<HubEventArgs<EndMyTurnMessage>>? EndTurnEvent;




        // Constructor
        public HubService(string authHeader, string baseUrl, string hubUri, bool clientDon = false)
        {
            hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{baseUrl}{hubUri}",
                    options =>
                    {
                        options.Headers.Add("Authorization", $"Basic {authHeader}");
                        options.Headers.Add("Access-Control-Allow-Origin", "*");
                    })
                    .Build();

            if (clientDon) // Only for web/don
            {
                hubConnection.On<MoveMessage>(Message.MessageType.MoveEvent.ToString(), msg =>
                {
                    MoveEvent?.Invoke(this, new HubEventArgs<MoveMessage>() { Messege = msg });
                });
                hubConnection.On<ActionMessage>(Message.MessageType.ActionEvent.ToString(), msg =>
                {
                    ActionEvent?.Invoke(this, new HubEventArgs<ActionMessage>() { Messege = msg });
                });

                hubConnection.On<GameSessionOptions>("JoinEvent", msg =>
                {
                    JoinEvent?.Invoke(this, new HubEventArgs<GameSessionOptions>() { Messege = msg });
                });

                hubConnection.On<GameSessionOptions>("LeaveEvent", msg =>
                {
                    LeaveEvent?.Invoke(this, new HubEventArgs<GameSessionOptions>() { Messege = msg });
                });

                hubConnection.On<EndMyTurnMessage>(Message.MessageType.EndMyTurn.ToString(), msg =>
                {
                    EndTurnEvent?.Invoke(this, new HubEventArgs<EndMyTurnMessage>() { Messege = msg });
                });


            }
            else  // Only for Mob users
            {
                hubConnection.On<StartGameMessage>(Message.MessageType.StartGame.ToString(), msg =>
                {
                    StartGame?.Invoke(this, new HubEventArgs<StartGameMessage>() { Messege = msg });
                });

                hubConnection.On<NewTurnMessage>(Message.MessageType.NewTurn.ToString(), msg =>
                {
                    NewTurnEvent?.Invoke(this, new HubEventArgs<NewTurnMessage>() { Messege = msg });
                });

                hubConnection.On<UpdateMessage>(Message.MessageType.UpdateEvent.ToString(), msg =>
                {
                    UpdateEvent?.Invoke(this, new HubEventArgs<UpdateMessage>() { Messege = msg });
                });

            }

            // Exception from hub system
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
            GameOptions = new GameSessionOptions() { SessionKey = key, Sheet = p };
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