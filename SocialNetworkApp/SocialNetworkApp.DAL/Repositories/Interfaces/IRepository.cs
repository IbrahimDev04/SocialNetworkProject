using SocialNetworkApp.Core.Entities.Commons;
using System.Linq.Expressions;

namespace SocialNetworkApp.DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity, new()
{
    public IQueryable<TEntity> GetAll();
    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression);
    public Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> expression);
    public Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);
    public Task CreateAsync(TEntity entity); 
    public Task SaveAsync();
}
