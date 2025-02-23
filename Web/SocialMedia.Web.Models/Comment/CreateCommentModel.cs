using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Comment
{
    public class CreateCommentModel
    {
        public string Content { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
