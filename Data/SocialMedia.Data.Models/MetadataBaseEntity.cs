using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public abstract class MetadataBaseEntity : BaseEntity
    {
        public string CreatedById { get; set; }
        public SocialMediaUser CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? UpdatedById { get; set; }
        public SocialMediaUser? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string? DeletedById { get; set; }
        public SocialMediaUser? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
