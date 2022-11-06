﻿using DevExpress.Data.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedClassLibrary;
using SharedClassLibrary.MessageStrings;

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
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = $"Joined room {options.SessionKey}" });
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = "Incompatible sesstion key" });
            }
        }

        public async Task LeaveGameRoom(GameSessionOptions options)
        {
            if (options is not null && !string.IsNullOrEmpty(options.SessionKey))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, options.SessionKey);
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = $"Left room {options.SessionKey}" });
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = "Incompatible sesstion key" });
            }
        }

        public async Task FileEvent(FileUpdateMessage msg) // From All to All
        {
            await Clients.OthersInGroup(msg.SessionKey).SendAsync("FileEvent", msg);
            //Debug echo
            await Clients.Client(Context.ConnectionId).SendAsync("ExceptionHandler", new HubServiceException() { Messege = msg.ToString() });
        }

        public async Task MoveEvent(StandardMessages msg) // To Don from Maui
        {
            msg.ConnectionId = Context.ConnectionId;
            await Clients.OthersInGroup(msg.SessionKey).SendAsync("MoveEvent", msg);
        }

        public async Task ErrorEvent(StandardMessages msg) // From Don to Maui
        {
            if (msg.ConnectionId != null)
            {
                await Clients.Client(msg.ConnectionId).SendAsync("ErrorEvent", msg);
            }
        }

        public async Task UpdateEvent(StandardMessages msg) // From Don to Maui
        {
            if (msg.ConnectionId != null) {
                await Clients.Client(msg.ConnectionId).SendAsync("UpdateEvent", msg);
            }
        }

        public async Task DiceEvent(StandardMessages msg) // To Don from Maui
        {
            msg.ConnectionId = Context.ConnectionId;
            await Clients.OthersInGroup(msg.SessionKey).SendAsync("DiceEvent", msg);
        }
    }
    
}