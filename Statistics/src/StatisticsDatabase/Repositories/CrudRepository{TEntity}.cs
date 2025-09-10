using Microsoft.EntityFrameworkCore;
using StatisticsDatabase.Context;

namespace StatisticsDatabase.Repositories;

public class CrudRepository<TEntity> : ICrudRepository<TEntity>
    where TEntity : class
{
    protected readonly StatisticsDatabaseContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public CrudRepository(StatisticsDatabaseContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return _dbSet.AsNoTracking().AsAsyncEnumerable();
    }

    public async Task<TEntity?> GetByIdOrDefaultAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync();
    }
}