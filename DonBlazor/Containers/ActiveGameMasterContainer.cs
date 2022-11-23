using DonBlazor.Client;
using SharedClassLibrary;
using SharedClassLibrary.MessageStrings;

namespace DonBlazor.Containers
{
    public sealed class ActiveGameMasterContainer : GameMaster
    {
        private static ActiveGameMasterContainer? GameMasterInstance = null;

        public static ActiveGameMasterContainer GetGameMasterInstance
        {
            get
            {
                GameMasterInstance ??= new ActiveGameMasterContainer();
                return GameMasterInstance;
            }
        }

        private ActiveGameMasterContainer()
        {

        }

        public string UserName { get; set; }
        public HubService Hub { get; set; }

        public async Task<bool> SetupHub(string roomName)
        {
            try
            {
                string authHeader = "dXNlcjpwYXNzd29yZA==";              // Should be set a global place
                string baseUrl = "https://dungun.azurewebsites.net";
                string hubUri = "/gamehub";
                bool clientDon = true;

                Hub = new HubService(authHeader, baseUrl, hubUri, clientDon);

                await Hub.Initialise();

                Hub.ExceptionHandlerEvent += (object? sender, HubEventArgs<HubServiceException> e) => Console.WriteLine(e.Messege?.Messege); // Subscribe to Exceptionhandler
                Hub.FileEvent += (object? sender, HubEventArgs<FileUpdateMessage> e) => Console.WriteLine(e.Messege?.ToString()); // Subscribe to Exceptionhandler
                Hub.JoinEvent += (object? sender, HubEventArgs<GameSessionOptions> e) =>
                {
                    Console.WriteLine(e.Messege?.Sheet);
                    var player = e.Messege?.Sheet;


                    AddCharacterToGame(e.Messege?.Sheet);
                    connectionsId.Add(e.Messege.Sheet.OwnerName, e.Messege.ConnectionId);

                    // TODO add to dictionary
                    // e.Messege.ConnectionId

                };
                // Hub.MoveEvent +=
                // Hub.ActionEvent +=
                // Hub.LeaveaEvent +=
                // Hub.EndTurn += 

                await Hub.JoinRoom(roomName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


    }
}
