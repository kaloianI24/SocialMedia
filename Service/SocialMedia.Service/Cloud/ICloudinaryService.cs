using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;
using SocialMedia.Service.Models;

namespace SocialMedia.Service.Cloud
{
    public interface ICloudinaryService : IGenericService<CloudResource, CloudResourceServiceModel>
    {
        Task<Dictionary<string, object>> UploadFile(IFormFile file);

    }
}
