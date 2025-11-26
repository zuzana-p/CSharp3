namespace ToDoList.Persistence.Repositories;

public interface IRepositoryAsync<T> where T : class
{
    public Task CreateAsync(T entity);
    public Task<IEnumerable<T>> ReadAsync();
    public Task<T?> ReadByIdAsync(int id);
    public Task UpdateByIdAsync(T entity);
    public Task DeleteByIdAsync(int id);
}
