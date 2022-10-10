using DevExpress.Data.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedClassLibrary;

namespace ServerSideApiSsl.Hubs
{
    [Authorize]
    public class FileHub : Hub
    {
        public async Task JoinGameRoom(GameSessionOptions options)
        {
            if (options is not null && !string.IsNullOrEmpty(options.SessionKey))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, options.SessionKey);
                await Clients.OthersInGroup(options.SessionKey).SendAsync("UserJoinedFileHub");
            }
            else
            {
                await Clients.User(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = "Incompatible sesstion key" });
            }
            
        }

        public async Task SendUpdateEvent(FileUpdateMessage msg)
        {
            await Clients.OthersInGroup(msg.SessionKey).SendAsync("ReceiveUpdateEvent", msg);
        }
    }
    
}
