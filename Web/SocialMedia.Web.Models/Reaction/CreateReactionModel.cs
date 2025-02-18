using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Reaction
{
    public class CreateReactionModel
    {
        public string Label { get; set; }

        public IFormFile Emote { get; set; }

    }
}
