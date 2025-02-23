using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Comment
{
    public interface ICommentService : IGenericService<SocialMediaComment, SocialMediaCommentServiceModel>
    {
    }
}
