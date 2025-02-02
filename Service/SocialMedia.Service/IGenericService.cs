namespace SocialMedia.Service
{
    public interface IGenericService<TEntity, TModel>
    {
        IQueryable<TModel> GetAll();

        TModel GetByIdAsync(string id);

        TModel CreateAsync(TModel model);
        
        TModel UpdateAsync(TModel model);

        TModel DeleteAsync(string id);
    }
}
