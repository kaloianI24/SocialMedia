namespace SocialMedia.Service.Models
{
    public abstract class MetadataBaseServiceModel : BaseServiceModel
    {
        public SocialMediaUser CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public SocialMediaUser UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public SocialMediaUser DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
