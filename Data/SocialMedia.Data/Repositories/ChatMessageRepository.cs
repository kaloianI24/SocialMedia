using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Models;
using SocialMedia.Web.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Repositories
{
    public class ChatMessageRepository
    {
        private readonly SocialMediaDbContext _context;

        public ChatMessageRepository(SocialMediaDbContext context)
        {
            _context = context;
        }

        public async Task<ChatMessage> SaveMessageAsync(string fromUserId, string toUserId, string encryptedText, string iv)
        {
            var message = new ChatMessage
            {
                SenderId = fromUserId,
                ReceiverId = toUserId,
                EncryptedText = encryptedText,
                IV = iv,
                SentAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public List<ChatMessage> GetMessageHistory(string fromUserId, string toUserId)
        {
            var query =  _context.ChatMessages
                .Where(m => (m.SenderId == fromUserId && m.ReceiverId == toUserId) ||
                            (m.SenderId == toUserId && m.ReceiverId == fromUserId))
                .OrderBy(m => m.SentAt);
                
             return query.ToList();
        }

        public async Task<List<UserConversationModel>> GetUserConversationsAsync(string currentUserId)
        {
            var chatMessages = await _context.ChatMessages
                .Include(m => m.Sender)
                .ThenInclude(s => s.ProfilePicture)
                .Include(m => m.Receiver)
                .ThenInclude(r => r.ProfilePicture)
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .ToListAsync();

            var grouped = chatMessages
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g =>
                {
                    var lastMessage = g.OrderByDescending(m => m.SentAt).FirstOrDefault();
                    var userId = g.Key;

                    var hasUnread = g.Any(m => m.ReceiverId == currentUserId && !m.IsRead);

                    var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                    return new UserConversationModel
                    {
                        UserId = userId,
                        UserName = user?.UserName,
                        ProfilePictureUrl = user?.ProfilePicture.CloudUrl,
                        LastMessageDate = lastMessage?.SentAt ?? DateTime.MinValue,
                        HasUnreadMessages = hasUnread
                    };
                })
                .OrderByDescending(x => x.LastMessageDate)
                .ToList();

            return grouped;
        }

        public async Task<List<ChatMessage>> UpdateStatus (List<ChatMessage> unreadMessages)
        {
            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }
            _context.ChatMessages.UpdateRange(unreadMessages);
            _context.SaveChanges();

            return unreadMessages;
        }

        public async Task<List<ChatMessage>> AllMessageById (string userId)
        {
             return await _context.ChatMessages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .ToListAsync();
        }

        public async Task DeleteAllMessages(List<ChatMessage> chatMessages)
        {
            if (chatMessages.Any())
            {
                _context.ChatMessages.RemoveRange(chatMessages);
                await _context.SaveChangesAsync();
            }            
        }
    }
}
