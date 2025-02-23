﻿using SocialMedia.Service.Models;

namespace SocialMedia.Service
{
    public interface IGenericService<TEntity, TModel>
    {
        IQueryable<TModel> GetAll();

        Task<TModel> GetByIdAsync(string id);

        Task<TModel> CreateAsync(TModel model);

        Task<TEntity> InternalCreateAsync(TEntity model);
        
        Task<TModel> UpdateAsync(TModel model);

        Task<TModel> DeleteAsync(string id);
    }
}
