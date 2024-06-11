using SocialNetworkApp.Core.Entities.Commons;

namespace SocialNetworkApp.DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity, new()
{
    public Task CreateAsync(TEntity entity);
    public Task SaveAsync();
}
