namespace TrackSense.API.AlertService.Services.CRUD;

public interface ICrudService<T> 
{
    public Task<IEnumerable<T>> GetAllAsync();

    public Task<T?> GetByIdOrDefaultAsync(int id);

    public Task<T> AddAsync(T entity);

    public Task<T> UpdateAsync(T entity);

    public Task<int> DeleteAsync(int id);
}