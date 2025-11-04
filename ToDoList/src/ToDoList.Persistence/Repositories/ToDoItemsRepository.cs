namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext dbContext) : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext dbContext = dbContext;

    public void Create(ToDoItem item)
    {
        dbContext.Add(item);
        dbContext.SaveChanges();
    }
}
