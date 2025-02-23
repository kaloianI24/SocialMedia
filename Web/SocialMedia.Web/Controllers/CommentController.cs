using Microsoft.AspNetCore.Mvc;
using SocialMedia.Service.Comment;
using SocialMedia.Service.Models;
using SocialMedia.Web.Models.Comment;

namespace SocialMedia.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            this._commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string postId, [FromBody]CreateCommentModel model)
        {
            var result = await this._commentService.CreateAsync(new SocialMediaCommentServiceModel
            {
                Content = model.Content
            });

            return Ok(result);
        }
    }
}
