using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Hub
{
    using Microsoft.AspNetCore.SignalR;

    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected User Identifier: " + Context.UserIdentifier);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"User disconnected: {Context.UserIdentifier}");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string userId, string message, DateTime createdAt)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message, DateTime.UtcNow.ToString("o"));
        }
    }
}
