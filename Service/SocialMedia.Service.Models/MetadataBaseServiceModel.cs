using SocialMedia.Areas.Identity.Data;

namespace SocialMedia.Service.Models
{
    public abstract class MetadataBaseServiceModel : BaseServiceModel
    {
        public SocialMediaUserServiceModel CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public SocialMediaUserServiceModel? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public SocialMediaUserServiceModel? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
