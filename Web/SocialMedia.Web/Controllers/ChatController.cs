using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Encryption;
using SocialMedia.Web.Models.Chat;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatMessageRepository _chatRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly UserManager<SocialMediaUser> _userManager;

        public ChatController(ChatMessageRepository chatRepository, IEncryptionService encryptionService, UserManager<SocialMediaUser> userManager)
        {
            _chatRepository = chatRepository;
            _encryptionService = encryptionService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Conversation(string userId)
        {
            var currentUser = await _userManager.Users
                .Include(u => u.ProfilePicture)
                .Include(u => u.Friends)
                .Include(u => u.BlockedUsers)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            var targetUser = await _userManager.Users
            .Include(u => u.ProfilePicture)
            .Include(u => u.Friends)
            .Include(u => u.Following)
            .Include(u => u.BlockedUsers)
            .FirstOrDefaultAsync(u => u.Id == userId);
        

            if(targetUser.IsPrivate && targetUser.Friends.Contains(currentUser) || targetUser.Following.Contains(currentUser) || !targetUser.IsPrivate && !targetUser.BlockedUsers.Contains(currentUser))
            {

                var messages = _chatRepository.GetMessageHistory(currentUser.Id, userId);
                var unreadMessages = messages
                    .Where(m => m.ReceiverId == currentUser.Id && !m.IsRead)
                    .ToList();

                await _chatRepository.UpdateStatus(unreadMessages);
                var history = new ChatViewModel
                {
                    ReceiverId = userId,
                    CurrentUserId = currentUser.Id,
                    ReceiverUserName = (await _userManager.FindByIdAsync(userId)).UserName,
                    Messages = messages.Select(m => new ChatMessageBasic
                    {
                        SenderId = m.SenderId,
                        SentAt = m.SentAt,
                        PlainText = _encryptionService.Decrypt(m.EncryptedText, m.IV)
                    }).ToList()
                };
                var role = await _userManager.GetRolesAsync(currentUser);
                ViewData["IsAdmin"] = role.Contains("Administrator");
                ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
                return View("Chat", history);
            }

            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> AllConversations()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allConversations = await _chatRepository.GetUserConversationsAsync(currentUser.Id);
            var role = await _userManager.GetRolesAsync(currentUser);
            ViewData["IsAdmin"] = role.Contains("Administrator");
            ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
            return View(allConversations);

        }
    }
}
