using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialMedia.Data.Models;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Mappings;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialMedia.Service.Cloud
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly IConfiguration configuration;

        private readonly ILogger<CloudinaryService> _logger;

        private readonly CloudResourceRepository cloudResourceRepository;

        private const string CloudinaryUrl = "https://api.cloudinary.com/v1_1/{0}/auto/upload";

        public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger, CloudResourceRepository cloudResourceRepository)
        {
            this.configuration = configuration;
            _logger = logger;
            this.cloudResourceRepository = cloudResourceRepository;
        }

        private string GetUnixTimestamp()
        {
            return ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
        }

        private string GetApiKey()
        {
            return configuration["Cloudinary:ApiKey"];
        }

        private string GetApiSecret()
        {
            return configuration["Cloudinary:ApiSecret"];
        }

        private string EncodeWithSha256(string value)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (byte b in result) stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
        }

        private string GetSignature(string timestamp, string publicId)
        {
            return EncodeWithSha256($"public_id={publicId}&timestamp={timestamp}{GetApiSecret()}");
        }

        private byte[] ReadFileBytes(IFormFile formFile)
        {
            if (formFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            return new byte[0];
        }

        public async Task<Dictionary<string, object>> UploadFile(IFormFile formFile)
        {
            var currentTimestamp = GetUnixTimestamp();
            var apiKey = GetApiKey();
            var publicId = Guid.NewGuid().ToString() + ":" + StripExtension(formFile.FileName);
            var signature = GetSignature(currentTimestamp, publicId);

            string file = Convert.ToBase64String(ReadFileBytes(formFile));
            var json = JsonConvert.SerializeObject(new
            {
                file = HttpUtility.HtmlEncode($"data:{formFile.ContentType};base64,") + file,
                timestamp = currentTimestamp,
                public_id = publicId,
                api_key = apiKey,
                signature
            });

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(CloudinaryUrl, configuration["Cloudinary:CloudName"]))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var httpClient = new HttpClient();
            var httpResponse = await httpClient.SendAsync(httpRequest);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseJson = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            }

            var deserializedResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(await httpResponse.Content.ReadAsStringAsync());
            _logger.LogError(deserializedResponse["error"].ToString());

            return null;
        }

        private string StripExtension(string fileName)
        {
            return fileName
                .Replace(".png", "")
                .Replace(".jpg", "")
                .Replace(".jpeg", "")
                .Replace(".gif", "")
                .Replace(".mp4", "")
                .Replace(".mkv", "");
        }

        public IQueryable<CloudResourceServiceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CloudResourceServiceModel> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<CloudResourceServiceModel> CreateAsync(CloudResourceServiceModel model)
        {
            CloudResource attachment = model.ToEntity();

            return (await cloudResourceRepository.CreateAsync(attachment)).ToModel();
        }

        public Task<CloudResource> InternalCreateAsync(CloudResource model)
        {
            throw new NotImplementedException();
        }

        public Task<CloudResourceServiceModel> UpdateAsync(CloudResourceServiceModel model)
        {
            throw new NotImplementedException();
        }

        public Task<CloudResourceServiceModel> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
