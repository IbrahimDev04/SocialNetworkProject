using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Entities.Commons;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.DAL.Repositories.Implements;

public class Repository<TEntity>(AppDbContext _context) : IRepository<TEntity> where TEntity : BaseEntity, new()
{
    DbSet<TEntity> Table = _context.Set<TEntity>();

    public async Task CreateAsync(TEntity entity)
    {
        await Table.AddAsync(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
