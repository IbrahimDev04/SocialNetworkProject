using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Entities.Commons;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SocialNetworkApp.DAL.Repositories.Implements;

public class Repository<TEntity>(AppDbContext _context) : IRepository<TEntity> where TEntity : BaseEntity, new()
{
    DbSet<TEntity> Table = _context.Set<TEntity>();

    public async Task CreateAsync(TEntity entity)
    {
        await Table.AddAsync(entity);
    }

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression)
    {
        return Table.Where(expression);
    }

    public IQueryable<TEntity> GetAll()
    {
        return Table.AsQueryable();
    }

    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await Table.FirstOrDefaultAsync(expression);
    }

    public async Task<bool> IsExist(Expression<Func<TEntity, bool>> expression)
    {
        return await Table.AnyAsync(expression);

    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
