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
    public enum CommentMappingsContext
    {
        Reaction,
        Parent,
        Reply,
        User
    }
    public static class SocialMediaCommentMappings
    {
            public static SocialMediaComment ToEntity(this SocialMediaCommentServiceModel model)
            {
                return new SocialMediaComment
                {
                    Content = model.Content,
                    Attachments = model.Attachments?.Select(attachment => attachment.ToEntity()).ToList(),
                };
            }

            public static SocialMediaCommentServiceModel ToModel(this SocialMediaComment entity, CommentMappingsContext context)
            {
                return new SocialMediaCommentServiceModel
                {
                    Id = entity.Id,
                    Content = entity.Content,
                    Attachments = entity.Attachments?.Select(attachment => attachment.ToModel()).ToList(),
                    Reactions = entity.Reactions?.Select(reaction => reaction.ToModel(UserCommentReactionMappingsContext.Comment)).ToList(),
                    Replies = entity.Replies?.Select(reply => reply.ToModel(CommentMappingsContext.Parent)).ToList(),
                    Parent = ShouldMapParent(context) ? entity.Parent?.ToModel(CommentMappingsContext.Reply) : null,
                    CreatedOn = entity.CreatedOn,
                    UpdatedOn = entity.UpdatedOn,
                    DeletedOn = entity.DeletedOn,
                    CreatedBy = ShouldMapUser(context) ? entity.CreatedBy.ToModel(UserPostMappingsContext.Post) : null,
                    UpdatedBy = ShouldMapUser(context) ? entity.UpdatedBy?.ToModel(UserPostMappingsContext.Post) : null,
                    DeletedBy = ShouldMapUser(context) ? entity.DeletedBy?.ToModel(UserPostMappingsContext.Post) : null
                };
            }
            private static bool ShouldMapReactions(CommentMappingsContext context)
            {
                return context == CommentMappingsContext.Parent
                    || context == CommentMappingsContext.Reply
                    || context == CommentMappingsContext.User;
            }

            private static bool ShouldMapReplies(CommentMappingsContext context)
            {
                return context == CommentMappingsContext.Parent
                    || context == CommentMappingsContext.Reaction
                    || context == CommentMappingsContext.User;
            }

            private static bool ShouldMapParent(CommentMappingsContext context)
            {
                return context == CommentMappingsContext.Reaction
                    || context == CommentMappingsContext.Reply
                    || context == CommentMappingsContext.User;
            }

            private static bool ShouldMapUser(CommentMappingsContext context)
            {
                return context == CommentMappingsContext.Reaction
                    || context == CommentMappingsContext.Parent
                    || context == CommentMappingsContext.Reply;
            }
        }
    }