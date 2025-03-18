using Microsoft.AspNetCore.Http;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Post
{
    public class UpdatePostWebModel
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public List<CloudResourceServiceModel> Attachments { get; set; }

        public List<string> TaggedUsersId { get; set; }
        public List<string>? TaggedUsersUserName { get; set; }

        public string? RemovedAttachmentIds { get; set; }
        public string? Tags { get; set; }
        public string? Visibility { get; set; }
    }
}
