using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Mappings
{
    public enum UserPostCommentMappingsContext
        {
            Post,
            Comment,
            User
        }

        public static class UserPostCommentMappings
        {
            public static UserPostCommentServiceModel ToModel(this UserPostComment entity, UserPostCommentMappingsContext context)
            {
                return new UserPostCommentServiceModel
                {
                    Id = entity.Id,
                    Comment = ShouldMapComments(context) ? entity.Comment?.ToModel(CommentMappingsContext.Parent) : null,
                    Post = ShouldMapPost(context) ? entity.Post?.ToModel(UserPostMappingsContext.User) : null,
                    User = ShouldMapUser(context) ? entity.User?.ToModel(UserPostMappingsContext.Post) : null
                };
            }

            private static bool ShouldMapComments(UserPostCommentMappingsContext context)
            {
                return context == UserPostCommentMappingsContext.Post || context == UserPostCommentMappingsContext.User;
            }

            private static bool ShouldMapPost(UserPostCommentMappingsContext context)
            {
                return context == UserPostCommentMappingsContext.Comment || context == UserPostCommentMappingsContext.User;
            }

            private static bool ShouldMapUser(UserPostCommentMappingsContext context)
            {
                return context == UserPostCommentMappingsContext.Post || context == UserPostCommentMappingsContext.Comment;
            }
        }
    }