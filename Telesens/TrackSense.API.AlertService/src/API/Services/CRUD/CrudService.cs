using TrackSense.API.AlertService.Repositories;

namespace TrackSense.API.AlertService.Services.CRUD;

public class CrudService<T> : ICrudService<T> where T : class
{
    protected readonly ILogger _logger;
    protected readonly ICrudRepository<T> _crudRepository;

    public CrudService(
        ILogger logger,
        ICrudRepository<T> crudRepository)
    {
        _logger = logger;
        _crudRepository = crudRepository;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _crudRepository.GetAllAsync();
    }

    public async Task<T?> GetByIdOrDefaultAsync(int id)
    {
        return await _crudRepository.GetByIdOrDefaultAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        return await _crudRepository.AddAsync(entity);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        return await _crudRepository.UpdateAsync(entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _crudRepository.GetByIdOrDefaultAsync(id);
        if (entity != null)
        {
            return await _crudRepository.DeleteAsync(entity);
        }
        return 0;
    }
}