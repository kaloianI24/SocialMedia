using SocialMedia.Data.Models;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Service.Mappings;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Reaction
{
    public class ReactionService : IReactionService
    {
        private readonly ReactionRepository reactionRepository;

        public ReactionService(ReactionRepository reactionRepository)
        {
            this.reactionRepository = reactionRepository;
        }

        public async Task<SocialMediaReactionServiceModel> CreateAsync(SocialMediaReactionServiceModel model)
        {
            SocialMediaReaction reaction = model.ToEntity();

            reaction = await this.reactionRepository.CreateAsync(reaction);

            return reaction.ToModel(UserPostMappingsContext.Reaction);
        }

        public Task<SocialMediaReactionServiceModel> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SocialMediaReactionServiceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SocialMediaReactionServiceModel> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SocialMediaReaction> InternalCreateAsync(SocialMediaReaction model)
        {
            throw new NotImplementedException();
        }

        public Task<SocialMediaReactionServiceModel> UpdateAsync(SocialMediaReactionServiceModel model)
        {
            throw new NotImplementedException();
        }
    }
}
