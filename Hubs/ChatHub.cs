using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();

        public override Task OnConnectedAsync()
        {            
            ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string connectionId, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", connectionId, user, message);
        }
    }
}
