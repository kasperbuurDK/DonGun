
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedClassLibrary;
using SharedClassLibrary.MessageStrings;
using System.Net;

namespace ServerSideApiSsl.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        public async Task JoinGameRoom(GameSessionOptions options)
        {
            if (options is not null && !string.IsNullOrEmpty(options.SessionKey))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, options.SessionKey);
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = $"Joined room {options.SessionKey}", ActionName = nameof(JoinGameRoom) });
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = "Incompatible sesstion key" , Code = (int)HttpStatusCode.NotAcceptable, ActionName = nameof(JoinGameRoom)});
            }
        }

        public async Task LeaveGameRoom(GameSessionOptions options)
        {
            if (options is not null && !string.IsNullOrEmpty(options.SessionKey))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, options.SessionKey);
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = $"Left room {options.SessionKey}", ActionName = nameof(LeaveGameRoom) });
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = "Incompatible sesstion key", Code = (int)HttpStatusCode.NotAcceptable, ActionName = nameof(LeaveGameRoom) });
            }
        }

        public async Task FileEvent(FileUpdateMessage msg) // From All to All
        {
            await Clients.OthersInGroup(msg.SessionKey).SendAsync("FileEvent", msg);
            //Debug echo
            await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = msg.ToString() });
        }

        public async Task MoveEvent(MoveMessage msg) // To Don from Maui
        {
            msg.ConnectionId = Context.ConnectionId;
            await Clients.OthersInGroup(msg.SessionKey).SendAsync("MoveEvent", msg);
            //Debug echo
            await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = msg.ToString() });
        }

        public async Task ErrorEvent(StandardMessages msg) // From Don to Maui
        {
            if (msg.ConnectionId != null)
            {
                await Clients.Client(msg.ConnectionId).SendAsync("ErrorEvent", msg);
                //Debug echo
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = msg.ToString() });
            }
        }

        public async Task UpdateEvent(UpdateMessage msg) // From Don to Maui
        {
            if (msg.ConnectionId != null) {
                await Clients.Client(msg.ConnectionId).SendAsync("UpdateEvent", msg);
                //Debug echo
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = msg.ToString() });
            }
        }

        public async Task StartGame(StartGameMessage msg) // From Don to Maui
        {
                await Clients.OthersInGroup(msg.SessionKey).SendAsync("StartGame", msg);
                //Debug echo
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = msg.ToString() });  
        }

        public async Task DiceEvent(DiceRolledMessage msg) // To Don from Maui
        {
            msg.ConnectionId = Context.ConnectionId;
            await Clients.OthersInGroup(msg.SessionKey).SendAsync("DiceEvent", msg);
            //Debug echo
            await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = msg.ToString() });
        }
    }
    
}
