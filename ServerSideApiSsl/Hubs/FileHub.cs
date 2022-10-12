using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedClassLibrary;

namespace ServerSideApiSsl.Hubs
{
    [Authorize]
    public class FileHub : Hub
    {
        public async Task SendUpdateEvent(FileUpdateMessage msg)
        {
            await Clients.All.SendAsync("ReceiveUpdateEvent", msg);
        }
    }
    
}
