using DonBlazor.Client;
using SharedClassLibrary;
using SharedClassLibrary.AuxUtils;
using SharedClassLibrary.MessageStrings;


namespace DonBlazor.Containers
{
    public class ActiveContainer
    {
        private GameMaster? _gameMaster;
        private Game? _game;
        private HubService? _hub;

        public GameMaster GameMaster
        {
            get
            {
                if (_gameMaster == null)
                {
                    _game = new Game();
                    _gameMaster = new GameMaster(_game);
                }

                return _gameMaster;
            }

            set
            {
                _gameMaster = value;
                NotifyGameMasterChanged();
            }
        }

        public Game Game
        {
            get
            {
                _game ??= GameMaster.Game;
                return _game;
            }
            set
            {
                NotifyGameChanged();
                _game = value;
            }
        }


        public event Action? GameMasterChanged;
        public event Action? GameChanged;
        public event Action? CharactersChanged;
        public event Action? CurrentCharacterChanged;
        public event Action? QueueHasChanged;

        public void NotifyGameMasterChanged() => GameMasterChanged?.Invoke();
        public void NotifyGameChanged() => GameChanged?.Invoke();
        public void NotifyCharactersChanged() => CharactersChanged?.Invoke();
        public void NotifyCurrentCharacterChanged() => CurrentCharacterChanged?.Invoke();
        public void NotifyQueueHasChanged() => QueueHasChanged?.Invoke();

        public HubService? Hub { get => _hub; set => _hub = value; }

        public async Task<bool> SetupHub(string roomName)
        {
            try
            {
                // TODO  Should be set a dynmic place
                string authHeader = "dXNlcjpwYXNzd29yZA==";              
                string baseUrl = "https://dungun.azurewebsites.net";
                string hubUri = "/gamehub";
                bool clientDon = true;

                _hub = new HubService(authHeader, baseUrl, hubUri, clientDon);

                await _hub.Initialise();

                _hub.ExceptionHandlerEvent += (object? sender, HubEventArgs<HubServiceException> e) => Console.WriteLine(e.Messege?.Messege); // Subscribe to Exceptionhandler
                _hub.FileEvent += (object? sender, HubEventArgs<FileUpdateMessage> e) => Console.WriteLine(e.Messege?.ToString()); 
                _hub.JoinEvent += async (object? sender, HubEventArgs<GameSessionOptions> e) =>
                {
                    Console.WriteLine(e.Messege?.Sheet);

                    if (e?.Messege?.Sheet != null)
                    {
                        Player newPlayer = e?.Messege?.Sheet;
                        if (GameMaster.ConnectionsId.ContainsKey(e.Messege.Sheet.OwnerName))
                        {

                            // User is already connected, do not allow user to have more than on character connected
                        }
                        else
                        {
                            newPlayer.Team = 1;
                            GameMaster.AddCharacterToGame(newPlayer);
                            GameMaster.ConnectionsId.Add(e.Messege.Sheet.OwnerName, e.Messege.ConnectionId);
                            NotifyCharactersChanged();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrogfully attempt to add player");
                        Console.WriteLine(e.ToString());
                    }

                    
                };
                _hub.MoveEvent += async (object? sender, HubEventArgs<MoveMessage> e) =>
                {
                    Console.WriteLine(e.Messege?.ToString());
                    if (e.Messege?.ConnectionId != null)
                    {
                        Player foundPlayer = FindPLayer(e.Messege.ConnectionId);
                        if (foundPlayer != null)
                        {
                            MoveDirections direction = Enum.Parse<MoveDirections>(e.Messege.Direction);
                          
                            var moveRespone = GameMaster.Move(foundPlayer, direction, e.Messege.Distence); 
                            if (moveRespone != StandardMessages.AllOK)
                            {
                                // error
                            }
                            else 
                            {
                                string actionsJson = GameMaster.PossibleActions.TypeToJson();
                                await Hub.Send(new UpdateMessage() 
                                { 
                                    ConnectionId = e.Messege.ConnectionId,
                                    PossibleActionsJson = actionsJson,
                                    UpdateStr = "Super" 
                                });

                            }
                            NotifyCurrentCharacterChanged();
                        }
                    }

                };
                _hub.ActionEvent += (object? sender, HubEventArgs<ActionMessage> e) =>
                {
                    Console.WriteLine(e.Messege?.ToString());
                };
               
                /*   Hub.LeaveEvent += (object? sender, HubEventArgs<ActionMessage> e) =>
                   {

                   };
                */
                // Hub.EndTurn += 

                await _hub.JoinRoom(roomName);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        private Player? FindPLayer(string connectionId)
        {
            string foundKey = GameMaster.ConnectionsId.FirstOrDefault(entry => EqualityComparer<string>.Default.Equals(entry.Value, connectionId)).Key;
            Player foundPLayer = Game.HumanPlayers.Where(x => x.OwnerName == foundKey).First();
            return foundPLayer ?? null;    
        }
    }
}
