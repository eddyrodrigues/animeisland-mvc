using AnimeIsland.Data.Context;
using AnimeIsland.Data.Contracts;
using AnimeIsland.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeIsland.Data.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
{
    protected readonly AnimeIslandDbContext _context;

    public BaseRepository(AnimeIslandDbContext context)
    {
        _context = context;
    }
    async Task<IEnumerable<TEntity>> IBaseRepository<TEntity>.GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    TEntity IBaseRepository<TEntity>.GetById(Guid id)
    {
        return _context.Set<TEntity>().Find(id);
    }
    TEntity IBaseRepository<TEntity>.Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public async Task Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    public TEntity Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        _context.SaveChanges();
        return entity;
    }
}