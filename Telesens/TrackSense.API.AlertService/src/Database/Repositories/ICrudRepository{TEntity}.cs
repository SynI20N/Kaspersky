using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrackSense.API.AlertService.Repositories;

/// <summary>
/// Generic repository for CRUD operations
/// </summary>
/// <typeparam name="TEntity">The model that is represented in the DatabaseContext</typeparam>
public interface ICrudRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdOrDefaultAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<int> DeleteAsync(TEntity entity);
}