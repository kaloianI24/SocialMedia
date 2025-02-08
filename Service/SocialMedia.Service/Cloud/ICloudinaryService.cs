using Microsoft.AspNetCore.Http;

namespace SocialMedia.Service.Cloud
{
    public interface ICloudinaryService
    {
        Task<Dictionary<string, object>> UploadFile(IFormFile file);
    }
}
