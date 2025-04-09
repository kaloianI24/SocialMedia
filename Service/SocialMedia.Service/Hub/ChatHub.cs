using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocialMedia.Service.Hub
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;
    using SocialMedia.Data.Repositories;
    using SocialMedia.Service.Encryption;
    using System.Collections.Concurrent;

    public class ChatHub : Hub
    {
        private readonly ChatMessageRepository _chatRepository;
        private readonly IServiceProvider _serviceProvider;
        private static ConcurrentDictionary<string, string> ConnectedUsers = new();

        public ChatHub(ChatMessageRepository chatRepository, IServiceProvider serviceProvider)
        {
            _chatRepository = chatRepository;
            _serviceProvider = serviceProvider;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
                ConnectedUsers[userId] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
                ConnectedUsers.TryRemove(userId, out _);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string toUserId, string plainText)
        {
            var fromUserId = Context.UserIdentifier;
            using var scope = _serviceProvider.CreateScope();
            var encryptionService = scope.ServiceProvider.GetRequiredService<IEncryptionService>();

            var encrypted = encryptionService.Encrypt(plainText);
            await _chatRepository.SaveMessageAsync(fromUserId, toUserId, encrypted.EncryptedText, encrypted.IV);

            if (ConnectedUsers.TryGetValue(toUserId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", fromUserId, plainText, DateTime.UtcNow.ToString("o"));
            }

            await Clients.Caller.SendAsync("ReceiveMessage", fromUserId, plainText, DateTime.UtcNow.ToString("o"));
        }
    }
}
