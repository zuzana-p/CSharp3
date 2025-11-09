namespace ToDoList.Persistence.Repositories;

public interface IRepository<T> where T : class
{
    public void Create(T entity);
    public IEnumerable<T> Read();
    public T? ReadById(int id);
    public void UpdateById(int id, T entity);
    public void DeleteById(int id);
}

