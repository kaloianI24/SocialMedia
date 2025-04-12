using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Repositories
{
    public class NotificationRepository
    {
        private readonly SocialMediaDbContext _context;

        public NotificationRepository(SocialMediaDbContext context)
        {
            _context = context;
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
        public List<Notification> GetAllForUser(string userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }

        public async Task DeleteAllNotifications(List<Notification> notifications)
        {
            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
                await _context.SaveChangesAsync();
            }
        }
    }
}
