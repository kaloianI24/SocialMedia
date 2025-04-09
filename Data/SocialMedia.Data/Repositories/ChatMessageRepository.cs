using SocialMedia.Data.Models;
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
    }
}
